import {Component, Input} from '@angular/core';
import {NgClass, NgStyle} from '@angular/common';

@Component({
  selector: 'app-chat-message',
  imports: [
    NgStyle
  ],
  templateUrl: './chat-message.component.html',
  styleUrl: './chat-message.component.scss'
})
export class ChatMessageComponent {
  @Input() content: string = "";
  @Input() id: string = "";
  @Input() author: string = "";
  @Input() isLeft: boolean = false;
}
