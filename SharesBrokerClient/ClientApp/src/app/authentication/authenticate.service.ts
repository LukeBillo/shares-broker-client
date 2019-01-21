import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { User } from '../shared/user.model';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class AuthenticateService {
  public get user(): User {
    return this._user;
  }

  private _user: User;

  private readonly authEndpoint = 'api/auth';
  private readonly port = 5000;

  constructor(private http: HttpClient, private router: Router) {
    this._user = null;
  }

  login(username: string, password: string): Observable<User> {
    if (this._user === null || this._user.username !== username) {
      this._user = new User(username, password);
    }

    return this.http.post(
      `http://localhost:${this.port}/${this.authEndpoint}`,
      {},
    ).pipe(map(_ => {
      this._user.isAuthenticated = true;
      return this.user;
    }));
  }

  logout() {
    this._user = null;
    this.router.navigate(['/login']);
  }
}
