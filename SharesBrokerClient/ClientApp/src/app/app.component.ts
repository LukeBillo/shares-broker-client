import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  isLoginPage: boolean;

  constructor(private router: Router) {
    this.router.events.subscribe(_ => {
      this.isLoginPage = this.router.url.includes('login');
    });
  }
}
