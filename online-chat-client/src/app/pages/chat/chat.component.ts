import {Component, OnInit} from '@angular/core';
import {ChatMessagesService} from '../../services/chat-message/chat-messages.service';
import {IMessage} from '../../services/chat-message/IMessage';
import {ChatMessageComponent} from '../../components/chat-message/chat-message.component';

@Component({
  selector: 'app-chat',
  imports: [
    ChatMessageComponent
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent implements OnInit {
  chatName: string = "Chat";
  messages: IMessage[] = [];
  constructor(private chatMessageService: ChatMessagesService) {}

  ngOnInit(): void {
    this.chatMessageService.getMessages().subscribe(m => this.messages = [...m])
  }
}
