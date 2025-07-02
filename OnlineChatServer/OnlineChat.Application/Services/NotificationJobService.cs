using Hangfire;
using Microsoft.Extensions.Logging;
using OnlineChat.Application.Abstractions;
using StackExchange.Redis;

namespace OnlineChat.Application.Services;

public class NotificationJobService : INotificationJobService {
	private readonly IBackgroundJobClient _backgroundJobClient;
	private readonly IDatabase _redis;
	private readonly INotificationService _notificationService;
	private readonly ILogger<NotificationJobService> _logger;
	private readonly int _minutesDelay = 5;

	public NotificationJobService(
		IBackgroundJobClient backgroundJobClient, 
		IConnectionMultiplexer redis,
		INotificationService notificationService,
		ILogger<NotificationJobService> logger
		) {
		_backgroundJobClient = backgroundJobClient;
		_redis = redis.GetDatabase();
		_notificationService = notificationService;
		_logger = logger;
	}
	
	public async Task ScheduleNotificationJob(Guid userId, Guid chatId) {
		_logger.LogDebug("Scheduling...");
		var jobString = $"notify:{userId}:{chatId}";
		
		var existingJob = await _redis.StringGetAsync(jobString);
		if (!existingJob.IsNullOrEmpty) return;
		
		var jobId = _backgroundJobClient
			.Schedule(
				() => _notificationService.NotifyUser(userId, chatId,  "Dear user, you have unread messages"),
				TimeSpan.FromMinutes(_minutesDelay)
				);
		
		await _redis.StringSetAsync(jobString, jobId, TimeSpan.FromMinutes(5));
	}
	
	public async Task CancelNotificationJob(Guid userId, Guid chatId) {
		_logger.LogDebug("Canceling schedule...");
		var jobString = $"notify:{userId}:{chatId}";
		
		var jobId = await _redis.StringGetAsync(jobString);
		if (jobId.IsNullOrEmpty) return;
		
		BackgroundJob.Delete(jobId);
		await _redis.KeyDeleteAsync(jobString);
	}
}