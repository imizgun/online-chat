import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import {IMessage} from '../chat-message/IMessage';
import {BehaviorSubject, Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatConnectionService {
  private hubConnection: signalR.HubConnection;
  private messagesSubject = new BehaviorSubject<IMessage[]>([]);
  messages$: Observable<IMessage[]> = this.messagesSubject.asObservable();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:5013/chats")
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connected to SignalR'))
      .catch((err: Error) => console.log(err));

    // this.hubConnection.on("JoinChat", )
  }
}
