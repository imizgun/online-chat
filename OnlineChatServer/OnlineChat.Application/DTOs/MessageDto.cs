using OnlineChat.DatabaseAccess.Abstraction;

namespace OnlineChat.Application.DTOs;

public class MessageDto : IIdentifiable
{
	public Guid Id { get; set; }
	public UserDto Author { get; set; }
	public Guid ChatId { get; set; }
	public string Content { get; set; } = "";
	public DateTime SentAt { get; set; }
}