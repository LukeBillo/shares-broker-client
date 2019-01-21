import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthenticateService } from './authenticate.service';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {
  private readonly forbidden = 403;
  private readonly unauthorized = 401;
  private loginAttempts = 0;

  constructor(private authenticationService: AuthenticateService, private router: Router, private route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      const attempts = params.get('loginAttempts');

      if (attempts === null) {
        this.loginAttempts = 0;
      } else {
        this.loginAttempts = parseInt(attempts, 10);
      }
    });
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(response => {
      if (response.status === this.unauthorized || response.status === this.forbidden) {
        this.authenticationService.logout();
        location.reload();
      }

      const error = response.error || response.statusText;
      return throwError(error);
    }));
  }
}
