export interface User {
    username: string;
    authenticated: boolean;
    shares: Share[];
}

export interface Share {
    companyName: string;
    companySymbol: string;
    numberOfShares: number;
    currency: string;
    value: number;
}