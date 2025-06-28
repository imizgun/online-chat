import { Injectable } from '@angular/core';
import {IMessage} from './IMessage';
import {BehaviorSubject, Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatMessagesService {
  private messagesArr: IMessage[] = [
    {
      id: "1",
      authorId: "1",
      authorName: "Alex",
      chatId: "1",
      chatName: "Chat",
      content: "HelloHelloHelloHelloHelloHelloHelloHelloHello",
      sentAt: "28.06.2025"
    }
  ];

  messageSub$ = new BehaviorSubject<IMessage[]>(this.messagesArr);

  constructor() {
  }

  getMessages(): Observable<IMessage[]> {
    return this.messageSub$.asObservable()
  }
}
