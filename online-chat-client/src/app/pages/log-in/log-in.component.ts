import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {Router, RouterLink} from '@angular/router';
import {IdentityService} from '../../services/identity-service/identity.service';
import {NgIf, NgStyle} from '@angular/common';

@Component({
  selector: 'app-log-in',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
    NgIf,
    NgStyle
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

  constructor(private identity: IdentityService, private navigator: Router) { }

  getErrorByKey(key: string): string {
    if (this.loginForm.controls[key].errors === null || Object.keys(this.loginForm.controls[key].errors ?? {}).length === 0) {
      return "";
    }
    else {
      let error = this.loginForm.controls[key].errors[Object.keys(this.loginForm.controls[key].errors ?? {})[0]];
      return error === true ? "This field is required" : error;
    }
  }

  responseObj = {
    responseText: "",
    isError: false,
  };
  isResponse = false;

  onSubmit(event: SubmitEvent) {
    event.preventDefault();

    if (this.loginForm.valid) {
      console.log(this.loginForm.value);

      this.identity.logInUser({
        email: this.loginForm.get('email')?.value ?? "",
        password: this.loginForm.get('password')?.value ?? ""

      }).subscribe({
        next: r => {

          sessionStorage.setItem('id', r.id);
          sessionStorage.setItem('email', r.email);
          sessionStorage.setItem('name', r.name);

          this.navigator.navigateByUrl("/chat_enter");
        },
        error: e => {
          this.responseObj.responseText = "Incorrect email or password";
          this.responseObj.isError = true;
          this.isResponse = true;
        }
      });
    }
  }

  protected readonly Object = Object;
}
