import { Share } from './share.model';

export class User {
    constructor(username: string, password: string) {
        this.credentials = btoa(`${username}:${password}`);
        this.isAuthenticated = false;
        this.shares = null;
        this.username = username;
    }

    username: string;
    credentials: string;
    isAuthenticated: boolean;
    shares: Share[];
    preferences: Preferences = {
      currency: 'GBP'
    };

    public static fromExistingCredentials(credentials: string): User {
      const decodedCredentials = atob(credentials).split(':');
      const username = decodedCredentials[0];
      const password = decodedCredentials[1];

      return new User(username, password);
    }
}

export interface Preferences {
  currency: string;
}
