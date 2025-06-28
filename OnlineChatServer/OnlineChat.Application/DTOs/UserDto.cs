using OnlineChat.DatabaseAccess.Abstraction;

namespace OnlineChat.Application.DTOs;

public class UserDto : IIdentifiable
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
}