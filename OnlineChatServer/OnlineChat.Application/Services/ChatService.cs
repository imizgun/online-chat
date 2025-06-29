using AutoMapper;
using OnlineChat.Application.Abstractions;
using OnlineChat.Application.DTOs;
using OnlineChat.DatabaseAccess.Abstraction;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.Application.Services;

public class ChatService : BaseService<Chat, ChatDto, IChatRepository>, IChatService
{
	public ChatService(IMapper mapper, IChatRepository repository) : base(mapper, repository)
	{ }

	public async Task<ChatDto?> GetChatByName(string name)
	{
		var res = await _repository.GetChatByName(name);

		if (res == null) return null;
		return _mapper.Map<ChatDto>(res);
	}

	public async Task<bool> AddUserToChat(Guid chatId, Guid userId)
	{
		return await _repository.AddUserToChat(chatId, userId);
	}

	public async Task<bool> IsUserInChat(Guid chatId, Guid userId)
	{
		return await _repository.IsUserInChat(chatId, userId);
	}

	public async Task<ChatDto?> GetPrivateChat(string email, string ownEmail)
	{
		var res = await _repository.GetPrivateChat(email, ownEmail);
 
		return res == null ? null : _mapper.Map<ChatDto>(res);
	}
}