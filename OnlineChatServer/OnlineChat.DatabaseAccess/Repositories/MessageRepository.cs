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
			.AsNoTracking()
			.Take(take)
			.Skip(skip * take)
			.Include(x => x.Author)
			.ToListAsync();
	}

	public override Task<Message?> GetAsync(Guid id)
	{
		return _context.Messages.AsNoTracking()
			.Include(x => x.Author)
			.Include(x => x.Chat)
			.FirstOrDefaultAsync(x => x.Id == id);
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

	public async Task<List<Message>> GetChatMessages(Guid chatId)
	{
		return await _context.Messages
			.Include(x => x.Author)
			.Include(x => x.Chat)
			.Where(x => x.ChatId == chatId)
			.OrderBy(x => x.SentAt)
			.ToListAsync();
	}
}