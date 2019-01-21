import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../shared/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthenticateService {
  public user: User;

  constructor(private http: HttpClient) {
    this.user = null;
  }

  login(username: string, password: string) {
    return this.http.post('');
  }
}
