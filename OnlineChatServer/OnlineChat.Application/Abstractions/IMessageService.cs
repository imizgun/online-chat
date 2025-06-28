using OnlineChat.Application.DTOs;

namespace OnlineChat.Application.Abstractions;

public interface IMessageService : IBaseService<MessageDto>
{
	Task<bool> UpdateAsync(Guid id, Guid changerId, string newContent);
}