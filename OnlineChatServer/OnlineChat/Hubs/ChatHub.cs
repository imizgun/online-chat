using Microsoft.AspNetCore.SignalR;
using OnlineChat.Models;

namespace OnlineChat.Hubs;

public class ChatHub : Hub
{
	public async Task JoinChat(UserConnection connection)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoomId.ToString());
	}
}