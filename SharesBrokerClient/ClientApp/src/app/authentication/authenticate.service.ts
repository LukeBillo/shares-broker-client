import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { User } from '../shared/user.model';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { Configuration } from '../shared/config';

/**
 * Authentication service; simply manages the SharesUser in
 * local memory and storage.
 * Auth headers added in BasicAuthInterceptor.
 */
@Injectable()
export class AuthenticateService {
  public get user(): User {
    return this._user;
  }

  private _user: User;

  private readonly authEndpoint = 'api/auth';

  constructor(private http: HttpClient, private router: Router) {
    this._user = null;
  }

  login(username: string, password: string): Observable<User> {
    if (this._user === null) {
      this._user = new User(username, password);
    }

    return this.http.post(
      `${Configuration.url}/${this.authEndpoint}`,
      {},
    ).pipe(map(_ => {
      this._user.isAuthenticated = true;
      localStorage.setItem('SharesUser', this._user.credentials);
      return this.user;
    }));
  }

  reauth(credentials: string): Observable<User> {
    if (this._user === null) {
      this._user = {
        credentials: credentials,
        isAuthenticated: false,
        shares: []
      };
    }

    return this.http.post(
      `${Configuration.url}/${this.authEndpoint}`,
      {},
    ).pipe(map(_ => {
      this._user.isAuthenticated = true;
      localStorage.setItem('SharesUser', this._user.credentials);
      return this.user;
    }));
  }

  logout() {
    this._user = null;
    this.router.navigate(['/login']);
  }
}
