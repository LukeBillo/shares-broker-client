import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthenticateService } from './authenticate.service';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { User } from '../shared/user.model';
import { map } from 'rxjs/operators';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private authenticateService: AuthenticateService, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
      if (this.authenticateService.user === null || !this.authenticateService.user.isAuthenticated) {
        const credentials = localStorage.getItem('SharesUser');

        if (credentials !== null) {
          return this.authenticateService.reauth(credentials).pipe(
            map((user: User) => {
              if (!user.isAuthenticated) {
                this.router.navigate(['/login']);
              }

              return user.isAuthenticated;
            }));
        }

        this.router.navigate(['/login']);
        return false;
      }

      return true;
  }
}
