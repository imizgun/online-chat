using Microsoft.EntityFrameworkCore;
using OnlineChat.DatabaseAccess.Abstraction;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess.Repositories;

public class ChatRepository : BaseRepository<Chat>, IChatRepository
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
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<Chat?> GetChatByName(string name)
	{
		return await _context.Chats
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Name == name);
	}

	public async Task<bool> AddUserToChat(Guid chatId, Guid userId)
	{
		var chat = await _context.Chats.FindAsync(chatId);
		var user = await _context.Users.FindAsync(userId);

		if (chat == null || user == null) return false;
		
		chat.Members.Add(user);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> IsUserInChat(Guid chatId, Guid userId)
	{
		var chat = await _context.Chats
			.Include(x => x.Members)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == chatId);
		
		return chat?.Members.Any(x => x.Id == userId) ?? false;
	}

	public async Task<Chat?> GetPrivateChat(string email, string ownEmail)
	{
		var chat = _context.Chats
			.Include(x => x.Members)
			.AsNoTracking()
			.Where(x => x.IsPrivate)
			.Where(x => x.Members.Any(u => u.Email == email) && x.Members.Any(u => u.Email == ownEmail));
		
		return await chat.FirstOrDefaultAsync();
	}
}