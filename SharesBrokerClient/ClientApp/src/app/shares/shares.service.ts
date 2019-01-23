import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Configuration } from '../shared/config';
import { SharesQuery, CreateHttpParams } from '../shared/sharesQuery.model';
import { Share } from '../shared/share.model';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class SharesService {
  private readonly sharesEndpoint = 'api/shares';

  constructor(private http: HttpClient) {}

  getShares(sharesQuery: Partial<SharesQuery>): Observable<Share[]> {
    return this.http.get<Share[]>(`${Configuration.url}/${this.sharesEndpoint}`, {
      params: CreateHttpParams(sharesQuery),
    });
  }

  buyShare(buyRequest: BuyShareRequest) {
    return this.http.post(`${Configuration.url}/${this.sharesEndpoint}`, buyRequest);
  }
}

export interface BuyShareRequest {
  buyerUsername: string;
  companySymbol: string;
  numberOfSharesToBuy: number;
}
