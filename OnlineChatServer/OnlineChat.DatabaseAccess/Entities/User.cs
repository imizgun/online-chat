using OnlineChat.DatabaseAccess.Abstraction;

namespace OnlineChat.DatabaseAccess.Entities;

public class User : IIdentifiable
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public string Email { get; set; } = "";
	public string Password { get; set; } = "";
	
	public List<Chat> Chats { get; set; } = new ();
	public List<Message> Messages { get; set; } = new ();
}