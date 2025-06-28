using Microsoft.EntityFrameworkCore;
using OnlineChat.DatabaseAccess.Abstraction;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess.Repositories;

public class ChatRepository : BaseRepository<Chat>
{
	public ChatRepository(ChatDbContext context) : base(context)
	{ }

	public override async Task<Guid> CreateAsync(Chat item)
	{
		_context.AttachRange(item.Members);
		return await base.CreateAsync(item);
	}

	public override Task<Chat?> GetAsync(Guid id)
	{
		return _context.Chats
			.Include(x => x.Members)
			.FirstOrDefaultAsync(x => x.Id == id);
	}
}