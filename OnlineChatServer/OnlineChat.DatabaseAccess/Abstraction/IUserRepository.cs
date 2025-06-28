using OnlineChat.DatabaseAccess.Entities;

namespace OnlineChat.DatabaseAccess.Abstraction;

public interface IUserRepository : IBaseRepository<User>
{
	Task<User?> GetByEmail(string email);
}