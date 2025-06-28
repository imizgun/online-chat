using Microsoft.EntityFrameworkCore;
using OnlineChat.DatabaseAccess.Abstraction;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess.Repositories;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
	public MessageRepository(ChatDbContext dbContext) : base(dbContext)
	{
		
	}
	public override async Task<Guid> CreateAsync(Message item)
	{
		_context.Attach(item.Author);
		_context.Attach(item.Chat);
		return await base.CreateAsync(item);
	}

	public override async Task<List<Message>> GetAllAsync(int skip, int take)
	{
		return await _context.Messages
			.Take(take)
			.Skip(skip * take)
			.Include(x => x.Author)
			.ToListAsync();
	}

	public async Task<bool> UpdateMessageAsync(Guid id, string newContent)
	{
		var res = await _context.Messages
			.Where(x => x.Id == id)
			.ExecuteUpdateAsync(s => s
				.SetProperty(p => p.Content, newContent)
			);

		return res > 0;
	}
}