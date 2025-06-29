namespace OnlineChat.Responses;

public class MessageResponse
{
	public Guid Id { get; set; }
	public UserResponse Author { get; set; }
	public ChatResponse Chat { get; set; }
	public string Content { get; set; } = "";
	public DateTime SentAt { get; set; }
}