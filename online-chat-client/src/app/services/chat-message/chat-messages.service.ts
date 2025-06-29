import { Injectable } from '@angular/core';
import {IMessage} from './IMessage';
import {BehaviorSubject, Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ChatMessagesService {
  private messagesArr: IMessage[] = [
    {
      id: "1",
      author: {
        id: "1",
        name: "Alex",
        email: "a@a.com"
      },
      chat: {
        chatId: "1",
        chatName: "Chat",
        isChatPrivate: false
      },
      content: "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello",
      sentAt: "28.06.2025"
    }
  ];

  constructor(private http: HttpClient) {
  }

  getMessages(chatId: string): Observable<IMessage[]> {
    return this.http.get<IMessage[]>(`http://localhost:5013/api/chats/${chatId}/messages`)
  }
}
