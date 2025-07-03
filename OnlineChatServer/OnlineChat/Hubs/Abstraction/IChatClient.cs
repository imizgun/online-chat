using OnlineChat.Responses;

namespace OnlineChat.Hubs.Abstraction;

public interface IChatClient {
		public Task ReceiveMessage(MessageResponse message);
		public Task MessageDeleted(Guid messageId);
		public Task MessageEdited(Guid messageId, string newContent);
}