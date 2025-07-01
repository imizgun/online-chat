namespace OnlineChat.Application.Abstractions;

public interface INotificationJobService {
	Task ScheduleNotificationJob(Guid userId, Guid chatId);

	Task CancelNotificationJob(Guid userId, Guid chatId);
}