using Microsoft.EntityFrameworkCore;
using OnlineChat.DatabaseAccess.Abstraction;
using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
	public UserRepository(ChatDbContext context) : base(context)
	{ }

	public async Task<User?> GetByEmail(string email)
	{
		return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
	}
}