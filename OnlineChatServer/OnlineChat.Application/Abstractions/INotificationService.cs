namespace OnlineChat.Application.Abstractions;

public interface INotificationService {
	Task NotifyUser(Guid userId, Guid chatId, string notificationMessage);
}