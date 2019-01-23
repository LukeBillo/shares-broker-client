import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticateService } from '../authentication/authenticate.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  errorMessage: string = null;
  pending = false;
  submitted = false;

  get username() {
    return this.loginForm.controls.username;
  }

  get password() {
    return this.loginForm.controls.password;
  }

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authenticateService: AuthenticateService
  ) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  login() {
    this.submitted = true;

    if (this.loginForm.invalid) {
      return;
    }

    this.pending = true;
    this.errorMessage = null;

    this.authenticateService.login(this.username.value, this.password.value).subscribe(user => {
      console.log(user);
      if (!user.isAuthenticated) {
        this.errorMessage = 'The username or password was incorrect.';
        return;
      }

      this.router.navigate(['']);
    });
  }
}
