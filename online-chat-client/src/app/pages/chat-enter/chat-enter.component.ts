import { Component } from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-chat-enter',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './chat-enter.component.html',
  styleUrl: './chat-enter.component.scss'
})
export class ChatEnterComponent {
  chatEnterForm: FormGroup = new FormGroup({
      chatName: new FormControl('', [Validators.required]),
      friendEmail: new FormControl('', [Validators.required, Validators.email])
    }
  );

  getErrorByKey(key: string): string {
    if (this.chatEnterForm.controls[key].errors === null || Object.keys(this.chatEnterForm.controls[key].errors ?? {}).length === 0) {
      return "";
    }
    else {
      let error = this.chatEnterForm.controls[key].errors[Object.keys(this.chatEnterForm.controls[key].errors ?? {})[0]];
      return error === true ? "This field is required" : error;
    }
  }

  onSubmit(event: SubmitEvent) {
    event.preventDefault();

    if (this.chatEnterForm.valid) {
      console.log(this.chatEnterForm.value);
    }
  }

  protected readonly Object = Object;
}
