import { Injectable } from '@angular/core';
import {IMessage} from './IMessage';
import {BehaviorSubject, Observable, tap} from 'rxjs';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ChatMessagesService {
  private messages: IMessage[] = [];
  messagesObs$ : BehaviorSubject<IMessage[]> = new BehaviorSubject<IMessage[]>(this.messages);
  constructor(private http: HttpClient) {
  }

  getMessages(chatId: string): Observable<IMessage[]> {
    return this.http.get<IMessage[]>(`http://localhost:5013/api/chats/${chatId}/messages`)
      .pipe(
        tap(msgs => {
          this.messages = msgs;
          this.messagesObs$.next(this.messages);
        })
      );
  }

  deleteMessage(messageId: string): void {
    this.messages = this.messages.filter(m => m.id !== messageId);
    this.messagesObs$.next([...this.messages]);
  }

  editMessage(messageId: string, newContent: string) : void {
    const indx = this.messages.findIndex(m => m.id === messageId);
    if (indx !== -1) {
      this.messages[indx].content = newContent;

      this.messagesObs$.next([...this.messages]);
    }
  }

  addMessage(message: IMessage) : void {
    this.messages = [...this.messages, message];

    this.messagesObs$.next([...this.messages]);
  }
}
