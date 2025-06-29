import {Component, ElementRef, NgZone, OnInit, ViewChild} from '@angular/core';
import {ChatMessagesService} from '../../services/chat-message/chat-messages.service';
import {IMessage} from '../../services/chat-message/IMessage';
import {ChatMessageComponent} from '../../components/chat-message/chat-message.component';
import {ActivatedRoute, Router} from '@angular/router';
import {ChatConnectionService} from '../../services/chat-connection-service/chat-connection.service';
import {FormsModule} from '@angular/forms';
import {filter, map} from 'rxjs';
import {NgClass} from '@angular/common';

@Component({
  selector: 'app-chat',
  imports: [
    ChatMessageComponent,
    FormsModule,
    NgClass
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent implements OnInit {
  chatName: string = "Chat";
  private chatId: string = ""
  messageText: string = "";
  messages: IMessage[] = [];
  @ViewChild('scrollAnchor') private scrollAnchor!: ElementRef;

  private scrollToBottom(): void {
    this.scrollAnchor.nativeElement.scrollIntoView({ behavior: 'smooth' });
  }

  constructor(private chatMessageService: ChatMessagesService,
              private router: ActivatedRoute,
              private chatConnection: ChatConnectionService) {}

  ngOnInit(): void {
    this.chatId = this.router.snapshot.paramMap.get('id')!;

    this.chatMessageService.getMessages(this.chatId).subscribe(history => {
      this.messages = history;
      this.chatName  = history[0]?.chat.chatName ?? 'Chat';
    });

    this.chatConnection.messages$
      .subscribe(m => {
        console.log('Received message from SignalR:', m);
          console.log(m.chat.chatId, this.chatId);
        if (m!.chat.chatId === this.chatId) {
          this.messages = [...this.messages, m];
          setTimeout(() => this.scrollToBottom(), 0)
        }
      });
  }

  sendMessage() {
    this.messageText = this.messageText.trim();
    console.log(sessionStorage.getItem("id") ?? "", this.chatId)
    console.log(this.messageText);
    if (this.messageText !== "") {
      this.chatConnection.sendMessage({
          UserId: sessionStorage.getItem("id") ?? "",
          ChatRoomId: this.chatId
        },
        this.messageText
      );
      this.messageText = "";
    }
  }

  protected readonly sessionStorage = sessionStorage;
}
