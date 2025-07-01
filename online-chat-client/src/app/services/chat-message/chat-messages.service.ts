import { Injectable } from '@angular/core';
import {IMessage} from './IMessage';
import {BehaviorSubject, Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ChatMessagesService {
  constructor(private http: HttpClient) {
  }

  getMessages(chatId: string): Observable<IMessage[]> {
    return this.http.get<IMessage[]>(`http://localhost:5013/api/chats/${chatId}/messages`)
  }
}
