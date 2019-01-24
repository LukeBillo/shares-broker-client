export interface Share {
    companyName: string;
    companySymbol: string;
    numberOfShares: number;
    sharePrice: SharePrice;
}

export interface SharePrice {
  currency: string;
  value: number;
}
