namespace OnlineChat.DatabaseAccess.Abstraction;

public interface IIdentifiable
{
	public Guid Id { get; set; }
}