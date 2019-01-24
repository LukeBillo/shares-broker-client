import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Configuration } from '../shared/config';
import { Share } from '../shared/share.model';

@Injectable({
  providedIn: 'root'
})
export class UserSharesService {
  private readonly userSharesEndpoint = 'api/user/shares';

  constructor(private http: HttpClient) {}

  getUserShares(username: string, currency?: string) {
    let httpParams = new HttpParams();
    httpParams = httpParams.append('username', username);

    if (currency) {
      httpParams = httpParams.append('currency', currency);
    }

    return this.http.get<Share[]>(`${Configuration.url}/${this.userSharesEndpoint}`, {
      params: httpParams,
    });
  }
}
