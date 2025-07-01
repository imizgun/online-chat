using System.Net;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using OnlineChat.Application.Abstractions;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace OnlineChat.Application.Services;

public class EmailNotificationService : INotificationService {
	private readonly IConfiguration _configuration;
	private readonly IChatService _chatService;
	private readonly ILogger<EmailNotificationService> _logger;

	public EmailNotificationService(IConfiguration configuration, IChatService chatService, ILogger<EmailNotificationService> logger) {
		_configuration = configuration;
		_chatService = chatService;
		_logger = logger;
	}
	
	public async Task NotifyUser(Guid userId, Guid chatId, string notificationMessage) {
		_logger.LogDebug("Sending email...");
		var chat = await _chatService.GetAsync(chatId);
		var user = chat!.Members.SingleOrDefault(x => x.Id == userId);
			
		var message = new MimeMessage();
		message.From.Add(new MailboxAddress("OnlineChat", _configuration["Smtp:ServerEmail"]));
		message.To.Add(new MailboxAddress(user!.Name, user.Email));
		message.Subject = "New unread messages from " + 
		                  (chat.IsPrivate ? $"your private chat with {chat.Members.FirstOrDefault(x => x.Id != userId)!.Name}" 
			                  : chat.Name);

		message.Body = new TextPart("html")
		{
			Text = $"<h1>Hello, {user.Name}</h1><p>{notificationMessage}</p>"
		};

		using var client = new SmtpClient();
		await client.ConnectAsync(_configuration["Smtp:Host"], Int32.Parse(_configuration["Smtp:Port"]!), SecureSocketOptions.StartTls);
		await client.AuthenticateAsync(_configuration["Smtp:ServerEmail"], _configuration["Smtp:EmailPassword"]);
		await client.SendAsync(message);
		await client.DisconnectAsync(true);
	}
}