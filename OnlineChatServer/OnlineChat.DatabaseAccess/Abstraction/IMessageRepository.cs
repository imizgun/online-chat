using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess.Abstraction;

public interface IMessageRepository : IBaseRepository<Message>
{
	Task<bool> UpdateMessageAsync(Guid id, string newContent);
	Task<List<Message>> GetChatMessages(Guid chatId);
}