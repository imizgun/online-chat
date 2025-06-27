import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {confirmPasswordValidator} from './repeat-password-validator';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-sign-up-page',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './sign-up-page.component.html',
  styleUrl: './sign-up-page.component.scss'
})
export class SignUpPageComponent implements OnInit {


  ngOnInit(): void {
    this.signUpForm.get("repeatPassword")?.setValidators(
      confirmPasswordValidator(confPass => confPass === this.signUpForm.get('password')?.value)
    )
  }

  signUpForm: FormGroup = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      name: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
      repeatPassword: new FormControl('', [Validators.required]),
    }
  );

  getErrorByKey(key: string): string {
    if (this.signUpForm.controls[key].errors === null || Object.keys(this.signUpForm.controls[key].errors ?? {}).length === 0) {
      return "";
    }
    else {
      let error = this.signUpForm.controls[key].errors[Object.keys(this.signUpForm.controls[key].errors ?? {})[0]];
      return error === true ? "This field is required" : error;
    }
  }

  onSubmit(event: SubmitEvent) {
    event.preventDefault();

    if (this.signUpForm.valid) {
      console.log(this.signUpForm.value);
    }
  }

  protected readonly Object = Object;
}
