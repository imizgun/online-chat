using AutoMapper;
using OnlineChat.Application.Abstractions;
using OnlineChat.Application.DTOs;
using OnlineChat.DatabaseAccess.Abstraction;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.Application.Services;

public class MessageService : BaseService<Message, MessageDto, IMessageRepository>,  IMessageService
{
	public MessageService(IMapper mapper, IMessageRepository repository) : base(mapper, repository)
	{ }

	public async Task<bool> UpdateAsync(Guid id, Guid changerId, string newContent)
	{
		var message = await _repository.GetAsync(id);
		
		if (message == null) return false;
		
		if (message.AuthorId != changerId) return false;

		return await _repository.UpdateMessageAsync(id, newContent);
	}

	public async Task<List<MessageDto>> GetChatMessages(Guid id)
	{
		var res = await _repository.GetChatMessages(id);
		return _mapper.Map<List<MessageDto>>(res);
	}
}