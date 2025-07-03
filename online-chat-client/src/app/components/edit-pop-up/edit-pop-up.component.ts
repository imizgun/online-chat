import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgClass} from '@angular/common';
import {NgClickOutsideDirective} from 'ng-click-outside2';
import {ChatConnectionService} from '../../services/chat-connection-service/chat-connection.service';
import {IUserConnection} from '../../services/chat-connection-service/IUserConnection';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-edit-pop-up',
  imports: [
    NgClass,
    NgClickOutsideDirective
  ],
  templateUrl: './edit-pop-up.component.html',
  styleUrl: './edit-pop-up.component.scss'
})
export class EditPopUpComponent {
  constructor(private chatConnection: ChatConnectionService, private router: ActivatedRoute) {
  }
  editing: boolean = false;
  @Output() onEditing: EventEmitter<boolean> = new EventEmitter<boolean>(this.editing);
  @Input() messageId: string = "";
  @Input() isActive: boolean = false;

  onClickOutSide(event: Event) {
    this.isActive = false;
    this.editing = false;
    // this.onEditing.emit(this.editing);
  }

  onDelete() {
    if (this.isActive) {
      let user: IUserConnection = {
        UserId: sessionStorage.getItem('id') ?? "",
        ChatRoomId: this.router.snapshot.paramMap.get('id') ?? ""
      }

      if (user.UserId !== null && user.ChatRoomId !== null) {
        this.chatConnection.deleteMessage(user, this.messageId);
      }
    }
  }

  onEdit() {
    if (this.isActive) {
      this.editing = true;
      this.onEditing.emit(this.editing);
    }
  }
}
