import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-log-in',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.scss'
})
export class LogInComponent  {
  loginForm: FormGroup = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required])
    }
  );

  getErrorByKey(key: string): string {
    if (this.loginForm.controls[key].errors === null || Object.keys(this.loginForm.controls[key].errors ?? {}).length === 0) {
      return "";
    }
    else {
      let error = this.loginForm.controls[key].errors[Object.keys(this.loginForm.controls[key].errors ?? {})[0]];
      return error === true ? "This field is required" : error;
    }
  }

  onSubmit(event: SubmitEvent) {
    event.preventDefault();

    if (this.loginForm.valid) {
      console.log(this.loginForm.value);
    }
  }

  protected readonly Object = Object;
}
