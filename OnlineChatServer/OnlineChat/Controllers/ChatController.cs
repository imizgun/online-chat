using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using OnlineChat.Application.Abstractions;
using OnlineChat.Application.DTOs;
using OnlineChat.Responses;

namespace OnlineChat.Controllers;

[ApiController]
[Route("api/chats")]
public class ChatController : ControllerBase
{
	private IChatService _chatService;
	private IUserService _userService;
	private IMessageService _messageService;
	private IMapper _mapper;
	private IDistributedCache _redis;
	
	public ChatController(
		IChatService chatService,
		IUserService userService, 
		IMessageService messageService, 
		IMapper mapper,
		IDistributedCache redis)
	{
		_chatService = chatService;
		_userService = userService;
		_messageService = messageService;
		_mapper = mapper;
		_redis = redis;
	}

	[HttpGet("join_public_chat")]
	public async Task<ActionResult<ChatIdResponse>> JoinPublicChat([FromQuery] string chatName, [FromQuery] Guid userId)
	{
		var existingChat = await _chatService.GetChatByName(chatName);

		if (existingChat != null)
		{
			if (!await _chatService.IsUserInChat(existingChat.Id, userId))
				await _chatService.AddUserToChat(existingChat.Id, userId);
			return Ok(new ChatIdResponse { Id = existingChat.Id });
		}
		
		var newChat = new ChatDto {
			Name = chatName,
			IsPrivate = false
		};

		var createdChatId = await _chatService.CreateAsync(newChat);
		await _chatService.AddUserToChat(createdChatId, userId);
		return Ok(new ChatIdResponse { Id = createdChatId });
	}
	
	[HttpGet("join_private_chat")]
	public async Task<ActionResult> JoinPrivateChat([FromQuery] string userEmail, [FromQuery] string ownEmail)
	{
		var existingUser = await _userService.GetByEmail(userEmail);
		var ownUser = await _userService.GetByEmail(ownEmail);

		if (existingUser == null || ownUser == null) return NotFound(new { message = "User with this email is not found" });
		var existingChat = await _chatService.GetPrivateChat(userEmail, ownEmail);

		if (existingChat != null) return Ok(new ChatIdResponse { Id = existingChat.Id });
		
		var newChat = new ChatDto {
			Name = $"{userEmail}___{ownEmail}",
			IsPrivate = true,
		};

		var createdChatId = await _chatService.CreateAsync(newChat);
		await _chatService.AddUserToChat(createdChatId, ownUser.Id);
		await _chatService.AddUserToChat(createdChatId, existingUser.Id);
		return Ok(new ChatIdResponse { Id = createdChatId });
	}

	[HttpGet("{id}/messages")]
	public async Task<ActionResult<List<MessageResponse>>> GetMessages([FromRoute] Guid id)
	{
		var chat = await _chatService.GetAsync(id);
		if (chat == null) return NotFound();
		
		return Ok(_mapper.Map<List<MessageResponse>>(await _messageService.GetChatMessages(id)));
	}
}