using OnlineChat.Application.DTOs;

namespace OnlineChat.Application.Abstractions;

public interface IUserService : IBaseService<UserDto>
{
	Task<UserDto?> Login(string email, string password);
	Task<UserDto?> GetByEmail(string email);
}