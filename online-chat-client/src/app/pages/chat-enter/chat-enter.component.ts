import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {ChatConnectionService} from '../../services/chat-connection-service/chat-connection.service';

@Component({
  selector: 'app-chat-enter',
  imports: [
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './chat-enter.component.html',
  styleUrl: './chat-enter.component.scss'
})
export class ChatEnterComponent implements OnInit {
  constructor(private router: Router, private activeRouter: ActivatedRoute, private chatConnection: ChatConnectionService) {}

  ngOnInit(): void {
    if (sessionStorage.getItem('id') === null) {
      sessionStorage.clear();
      this.router.navigate(['/log_in']);
      return;
    }
  }

  publicChatEnterForm: FormGroup = new FormGroup({
      chatName: new FormControl('', [Validators.required])
    }
  );

  privateChatEnterForm: FormGroup = new FormGroup({
    friendEmail: new FormControl('', [Validators.required, Validators.email])
  })

  getErrorByKey(key: string, form: FormGroup): string {
    if (form.controls[key].errors === null || Object.keys(form.controls[key].errors ?? {}).length === 0) {
      return "";
    }
    else {
      let error = form.controls[key].errors[Object.keys(form.controls[key].errors ?? {})[0]];
      return error === true ? "This field is required" : error;
    }
  }

  onSubmit(event: SubmitEvent, form: FormGroup, isPublic: boolean) {
    event.preventDefault();

    if (form.valid) {
      console.log(form.value);
      if (isPublic)
        this.chatConnection.joinPublicChat(form.value['chatName'], sessionStorage.getItem('id') ?? "")
      else if (form.value['friendEmail'] !== sessionStorage.getItem('email')) {
        this.chatConnection.joinPrivateChat(
          form.value['friendEmail'],
          sessionStorage.getItem("email") ?? "",
          sessionStorage.getItem('id') ?? "")
      }
      else {
        alert("You can't chat yourself")
      }
    }
  }

  protected readonly Object = Object;
}
