import { Injectable, isDevMode } from '@angular/core';
import {
  HttpInterceptor,
  HttpEvent,
  HttpRequest,
  HttpHandler,
  HttpErrorResponse,
} from '@angular/common/http';
import { catchError, Observable, of, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { redirectToLoginIfNeeded } from './utils';

@Injectable()
export class AuthHttpInterceptor implements HttpInterceptor {
  constructor(
    private readonly router: Router,
    private readonly authService: AuthService,
  ) {}

  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {    
    const transformedHttpRequest = isDevMode()
      ? httpRequest.clone({
          withCredentials: true,
        })
      : httpRequest;

    return next.handle(transformedHttpRequest).pipe(
      catchError((err: HttpErrorResponse) => {
        if (redirectToLoginIfNeeded(err)) {
          return new Observable<any>();
        }

        if (err.status == 401 || err.status == 403) {
          this.authService.authorized.next(false);
          this.router.navigate(['_/unauthorized']);
          return of(null);
        }

        return throwError(() => err);
      }),
    );
  }
}
