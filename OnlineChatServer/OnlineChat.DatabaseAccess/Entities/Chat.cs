using OnlineChat.DatabaseAccess.Abstraction;

namespace OnlineChat.DatabaseAccess.Entities;

public class Chat : IIdentifiable
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public bool IsPrivate { get; set; }
	
	public List<User> Members { get; set; } = new ();
	public List<Message> Messages { get; set; } = new();
}