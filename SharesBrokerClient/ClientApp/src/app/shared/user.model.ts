import { Share } from './share.model';

export class User {
    constructor(username: string, password: string) {
        this.username = username;
        this.password = password;
        this.isAuthenticated = false;
        this.shares = null;
    }

    username: string;
    password: string;
    isAuthenticated: boolean;
    shares: Share[];
}