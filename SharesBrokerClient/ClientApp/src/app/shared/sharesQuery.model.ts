import { Currency } from './currency.enum';
import { HttpParams } from '@angular/common/http';

export interface SharesQuery {
  companySymbol: string;
  companyName: string;
  availableSharesLessThan: number;
  availableSharesMoreThan: number;
  priceLessThan: number;
  priceMoreThan: number;
  priceLastUpdatedBefore: Date;
  priceLastUpdatedAfter: Date;
  currency: Currency;
  limit: number;
}

export function CreateHttpParams(sharesQuery: Partial<SharesQuery>): HttpParams {
  let params = new HttpParams();

  for (const key in sharesQuery) {
    if (sharesQuery.hasOwnProperty(key)) {
      const value = sharesQuery[key];

      if (value != null) {
        params = params.append(key, value);
      }
    }
  }

  return params;
}
