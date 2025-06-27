using OnlineChat.DatabaseAccess.Abstraction;

namespace OnlineChat.DatabaseAccess.Entities;

public class Message : IIdentifiable
{
	public Guid Id { get; set; }
	
	public User Author { get; set; }
	public Guid AuthorId { get; set; }
	
	public Chat Chat { get; set; }
	public Guid ChatId { get; set; }
	
	public string Content { get; set; } = "";
	public DateTime SentAt { get; set; }
}