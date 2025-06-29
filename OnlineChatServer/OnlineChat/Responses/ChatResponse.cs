namespace OnlineChat.Responses;

public class ChatResponse
{
	public Guid ChatId { get; set; }
	public string ChatName { get; set; }
	public bool IsChatPrivate { get; set; }
}