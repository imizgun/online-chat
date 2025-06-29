using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess.Abstraction;

public interface IChatRepository : IBaseRepository<Chat>
{
	Task<Chat?> GetChatByName(string name);

	Task<bool> AddUserToChat(Guid chatId, Guid userId);
	
	Task<bool> IsUserInChat(Guid chatId, Guid userId);
	
	Task<Chat?> GetPrivateChat(string email, string ownEmail);
}