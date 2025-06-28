using OnlineChat.DatabaseAccess.Abstraction;

namespace OnlineChat.Application.DTOs;

public class ChatDto : IIdentifiable
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public bool IsPrivate { get; set; }
	public List<UserDto> Members { get; set; } = new ();
	public List<MessageDto> Messages { get; set; } = new();
}