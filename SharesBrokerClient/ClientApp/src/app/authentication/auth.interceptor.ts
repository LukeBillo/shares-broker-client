import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticateService } from './authenticate.service';

@Injectable()
export class BasicAuthInterceptor implements HttpInterceptor {
  constructor(private authenticateService: AuthenticateService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const currentUser = this.authenticateService.user;
    if (currentUser != null) {
      const encodedCredentials = btoa(`${currentUser.username}:${currentUser.password}`);

      request = request.clone({
        setHeaders: {
          Authorization: `Basic ${encodedCredentials}`
        }
      });
    }

    return next.handle(request);
  }
}
