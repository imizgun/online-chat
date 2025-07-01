import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import {IMessage} from '../chat-message/IMessage';
import {BehaviorSubject, Observable, Subject} from 'rxjs';
import {IUserConnection} from './IUserConnection';
import {HttpClient} from '@angular/common/http';
import {IPublicChat} from './IPublicChat';
import {Router} from '@angular/router';
import {ChatMessagesService} from '../chat-message/chat-messages.service';

@Injectable({
  providedIn: 'root'
})
export class ChatConnectionService {
  private readonly hubConnection: signalR.HubConnection;
  private messagesSubject: Subject<IMessage> = new Subject<IMessage>();
  messages$: Observable<IMessage> = this.messagesSubject.asObservable();
  private privateChatUrl = "join_private_chat";
  private publicChatUrl = "join_public_chat";

  constructor(private http: HttpClient, private router: Router, private chatMessages: ChatMessagesService) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5013/chat_hubs")
      .withAutomaticReconnect([0, 1000, 2000, 5000])
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('[Signalr]: Connected to SignalR'))
      .catch((err: Error) => console.log(err));

    this.hubConnection.on("ReceiveMessage", (msg: IMessage) => {
      this.messagesSubject.next(msg)
    })

    this.hubConnection.onreconnecting((error) => {
      console.log("[Signalr]: Reconnecting...", error);
    });

    this.hubConnection.onreconnected((connectionId) => {
      console.log("[Signalr]: Reconnected! Connection ID:", connectionId);
    });

    this.hubConnection.onclose((error) => {
      console.log("[Signalr]: Connection closed", error);
    });
  }

  joinPublicChat(chatName: string, userId: string) {
    this.http.get<{id: string}>(`http://localhost:5013/api/chats/${this.publicChatUrl}?chatName=${chatName}&userId=${userId}`)
      .subscribe({
        next : r => this.invokeJoin(r, userId)
      })
  }

  joinPrivateChat(userEmail: string, ownEmail: string, userId: string) {
    this.http.get<{id: string}>(`http://localhost:5013/api/chats/${this.privateChatUrl}?userEmail=${userEmail}&ownEmail=${ownEmail}`)
      .subscribe({
        next : r => this.invokeJoin(r, userId)
      })
  }

  private async invokeJoin(r: {id: string}, u: string) {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      await this.hubConnection.start();
    }
    await this.hubConnection.invoke("JoinChat", {userId: u, chatRoomId: r.id})
    console.log(this.hubConnection);
    this.router.navigate(["/chats", r.id]);
  }

  sendMessage(userConnection: IUserConnection, message: string) {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) return;

    this.hubConnection.invoke("SendMessage", userConnection, message);
  }
}
