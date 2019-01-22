import { Share } from './share.model';

export class User {
    constructor(username: string, password: string) {
        this.credentials = btoa(`${username}:${password}`);
        this.isAuthenticated = false;
        this.shares = null;
    }

    credentials: string;
    isAuthenticated: boolean;
    shares: Share[];
}
