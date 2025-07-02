import {Component, ElementRef, Input, ViewChild} from '@angular/core';
import {NgClass, NgStyle} from '@angular/common';
import {EditPopUpComponent} from '../edit-pop-up/edit-pop-up.component';
import {ChatMessagesService} from '../../services/chat-message/chat-messages.service';
import {ChatConnectionService} from '../../services/chat-connection-service/chat-connection.service';
import {IUserConnection} from '../../services/chat-connection-service/IUserConnection';

@Component({
  selector: 'app-chat-message',
  imports: [
    NgStyle,
    NgClass,
    EditPopUpComponent
  ],
  templateUrl: './chat-message.component.html',
  styleUrl: './chat-message.component.scss'
})
export class ChatMessageComponent {
  @Input() content: string = "";
  @Input() id: string = "";
  @Input() author: string = "";
  @Input() isLeft: boolean = false;
  @Input() sentAt: string = "";
  @Input() chatId: string = "";
  @ViewChild('editableDiv') editableDiv!: ElementRef<HTMLDivElement>;
  isEditing = false;
  isActivePopUp = false;

  constructor(private chatConnection: ChatConnectionService) {
  }

  setPopUp() {
    this.isActivePopUp = !this.isActivePopUp;
  }

  saveEdit() {
    if (this.editableDiv.nativeElement.textContent !== "" || this.editableDiv.nativeElement.textContent !== null) {
      this.content = this.editableDiv.nativeElement.textContent ?? ""
      let userConnection: IUserConnection = {
        UserId: sessionStorage.getItem('id') ?? "",
        ChatRoomId: this.chatId
      }
      this.chatConnection.editMessage(userConnection, this.id, this.content);
      this.setEditContent(false)
    }
  }

  setEditContent(isEditing: boolean) {
    this.isEditing = isEditing;
    if (!this.isEditing) {
      this.editableDiv.nativeElement.textContent = this.content;
    }
  }
}
