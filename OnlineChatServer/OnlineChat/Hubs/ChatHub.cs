using Microsoft.AspNetCore.SignalR;
using OnlineChat.Application.Abstractions;
using OnlineChat.Application.DTOs;
using OnlineChat.Models;
using OnlineChat.Responses;
using StackExchange.Redis;

namespace OnlineChat.Hubs;

public interface IChatClient
{
	public Task ReceiveMessage(MessageResponse message);
}

public class ChatHub : Hub<IChatClient>
{
	private readonly IUserService _userService;
	private readonly IChatService _chatService;
	private readonly IMessageService _messageService;
	private IDatabase _redis;
	private INotificationJobService _notificationJobService;

	public ChatHub(
		IUserService userService, 
		IChatService chatService, 
		IMessageService messageService,
		IConnectionMultiplexer redis,
		INotificationJobService notificationJobService)
	{
		_userService = userService;
		_chatService = chatService;
		_messageService = messageService;
		_redis = redis.GetDatabase();
		_notificationJobService = notificationJobService;
	}
	
	public async Task JoinChat(UserConnection connection)
	{
		var user = await _userService.GetAsync(connection.UserId);
		var chatRoom = await _chatService.GetAsync(connection.ChatRoomId);
		if (user == null || chatRoom == null) return;
		
		await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoomId.ToString());
		
		await SetUserDataInRedis(user.Id, Context.ConnectionId);

		await _notificationJobService.CancelNotificationJob(connection.UserId, connection.ChatRoomId);

		var message = $"{user.Name} joined the chat";

		var messageDto = new MessageDto {
			Content = message,
			SentAt = DateTime.UtcNow,
			Author = new UserDto { Id = connection.UserId },
			Chat = new ChatDto { Id = chatRoom.Id }
		};
		
		var id = await _messageService.CreateAsync(messageDto);
		var mess = await _messageService.GetAsync(id);
		var messResponse = new MessageResponse {
			Content = mess!.Content,
			SentAt = mess.SentAt,
			Id = mess.Id,
			Author = new UserResponse {
				Id = mess.Author.Id,
				Name = mess.Author.Name,
				Email = mess.Author.Email
			},
			Chat = new ChatResponse {
				ChatId = mess.Chat.Id,
				ChatName = mess.Chat.Name,
				IsChatPrivate = mess.Chat.IsPrivate,
			}
		};
		
		await Clients
			.Group(connection.ChatRoomId.ToString())
			.ReceiveMessage(messResponse);
		
		await ScheduleNotification(connection.UserId, connection.ChatRoomId);
	}

	public override async Task OnDisconnectedAsync(Exception? exception) {
		var connection = await _redis.StringGetAsync($"connection:{Context.ConnectionId}");
		if (!connection.IsNullOrEmpty) {
			var deserialized = Guid.Parse(connection!);
			await DeleteUserDataFromRedis(deserialized, Context.ConnectionId);
		}
		await base.OnDisconnectedAsync(exception);
	}

	public async Task SendMessage(UserConnection connection, string message)
	{
		var user = await _userService.GetAsync(connection.UserId);
		
		if (user == null) return;
		
		await SetUserDataInRedis(user.Id, Context.ConnectionId);
		
		var messageDto = new MessageDto {
			Content = message,
			SentAt = DateTime.UtcNow,
			Author = new UserDto { Id = connection.UserId },
			Chat = new ChatDto { Id = connection.ChatRoomId }
		};
		
		var id = await _messageService.CreateAsync(messageDto);
		var mess = await _messageService.GetAsync(id);

		var messResponse = new MessageResponse {
			Content = mess!.Content,
			SentAt = mess.SentAt,
			Id = mess.Id,
			Author = new UserResponse {
				Id = mess.Author.Id,
				Name = mess.Author.Name,
				Email = mess.Author.Email
			},
			Chat = new ChatResponse {
				ChatId = mess.Chat.Id,
				ChatName = mess.Chat.Name,
				IsChatPrivate = mess.Chat.IsPrivate,
			}
		};
		
		await Clients
			.Group(connection.ChatRoomId.ToString())
			.ReceiveMessage(messResponse);

		await ScheduleNotification(connection.UserId, connection.ChatRoomId);
	}

	private async Task ScheduleNotification(Guid userId, Guid chatRoomId) {
		var chat = await _chatService.GetAsync(chatRoomId);
		
		if (chat == null) return;

		var requests = chat.Members.Select(async m => {
			var key = await _redis.StringGetAsync($"user:{m.Id}:connection");
			if (!key.IsNullOrEmpty) return;
			
			if (userId != m.Id)
				await _notificationJobService.ScheduleNotificationJob(m.Id, chatRoomId);
		});

		await Task.WhenAll(requests);
	}

	private async Task SetUserDataInRedis(Guid userId, string connectionId, int ttlMins = 5) {
		await _redis.StringSetAsync($"connection:{connectionId}", userId.ToString(), TimeSpan.FromMinutes(ttlMins));
		await _redis.StringSetAsync($"user:{userId}:connection", connectionId, TimeSpan.FromMinutes(ttlMins));
	}

	private async Task DeleteUserDataFromRedis(Guid userId, string connectionId) {
		await _redis.KeyDeleteAsync($"connection:{connectionId}");
		await _redis.KeyDeleteAsync($"user:{userId}:connection");
	}
}