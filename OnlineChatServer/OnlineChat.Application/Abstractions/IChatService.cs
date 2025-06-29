using OnlineChat.Application.DTOs;

namespace OnlineChat.Application.Abstractions;

public interface IChatService : IBaseService<ChatDto>
{
	Task<ChatDto?> GetChatByName(string name);
	Task<bool> AddUserToChat(Guid chatId, Guid userId);
	Task<bool> IsUserInChat(Guid chatId, Guid userId);
	Task<ChatDto?> GetPrivateChat(string email, string ownEmail);
}