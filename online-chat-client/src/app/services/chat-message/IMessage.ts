export interface IMessage {
  id: string;
  author: {
    id: string;
    name: string;
    email: string;
  }
  chat: {
    chatId: string;
    chatName: string;
    isChatPrivate: boolean;
  }
  content: string;
  sentAt: string;
}
