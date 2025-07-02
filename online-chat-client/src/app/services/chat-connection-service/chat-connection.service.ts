import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import {IMessage} from '../chat-message/IMessage';
import {Observable, Subject} from 'rxjs';
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
      this.chatMessages.addMessage(msg)
    })

    this.hubConnection.on("MessageDeleted", (messageId) => {
      this.chatMessages.deleteMessage(messageId);
    })

    this.hubConnection.on("MessageEdited", (messageId: string, newContent)=> {
      this.chatMessages.editMessage(messageId, newContent);
    });

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

  deleteMessage(userConnection: IUserConnection, messageId: string) {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) return;

    this.hubConnection.invoke("DeleteMessage", userConnection, messageId);
  }

  editMessage(userConnection: IUserConnection, messageId: string, newContent: string) {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) return;

    this.hubConnection.invoke("EditMessage", userConnection, messageId, newContent);
  }

  async rejoinChat(chatRoomId: string, userId: string) {
    console.log("[Rejoin]: Trying to rejoin", chatRoomId, userId);

    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      const connected = await this.waitUntilConnected(5000);
      if (!connected) {
        console.warn("[Rejoin]: Timed out waiting for connection");
        return;
      }
    }

    try {
      await this.hubConnection.invoke("JoinChat", { userId, chatRoomId });
      console.log("[Rejoin]: Successfully rejoined", chatRoomId);
    } catch (err) {
      console.error("[Rejoin]: Failed to invoke JoinChat", err);
    }
  }

  private async waitUntilConnected(timeoutMs = 5000): Promise<boolean> {
    const start = Date.now();
    while (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      if (Date.now() - start > timeoutMs) return false;
      await new Promise(resolve => setTimeout(resolve, 100));
    }
    return true;
  }

}
