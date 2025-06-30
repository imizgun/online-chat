using Microsoft.AspNetCore.SignalR;
using OnlineChat.Application.Abstractions;
using OnlineChat.Application.DTOs;
using OnlineChat.Models;
using OnlineChat.Responses;

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

	public ChatHub(IUserService userService, IChatService chatService,  IMessageService messageService)
	{
		_userService = userService;
		_chatService = chatService;
		_messageService = messageService;
	}
	
	public async Task JoinChat(UserConnection connection)
	{
		var user = await _userService.GetAsync(connection.UserId);
		var chatRoom = await _chatService.GetAsync(connection.ChatRoomId);
		if (user == null || chatRoom == null) return;
		
		await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoomId.ToString());

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
	}

	public async Task SendMessage(UserConnection connection, string message)
	{
		var user = await _userService.GetAsync(connection.UserId);
		
		if (user == null) return;
		
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
	}
}