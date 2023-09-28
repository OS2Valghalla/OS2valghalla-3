import { Injectable, isDevMode } from '@angular/core';
import {
  HttpInterceptor,
  HttpEvent,
  HttpRequest,
  HttpHandler,
  HttpErrorResponse,
  HttpClient,
} from '@angular/common/http';
import { catchError, Observable, of, take, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { COOKIE_EXPIRED_MESSAGE, IFRAME_MESSAGE } from './consts';

@Injectable()
export class AuthHttpInterceptor implements HttpInterceptor {
  constructor(
    private readonly httpClient: HttpClient,
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
        if (err.status == 401 && err.error == COOKIE_EXPIRED_MESSAGE) {
          return this.refreshCookie(httpRequest);
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

  private refreshCookie(originalHttpRequest: HttpRequest<any>) {
    return new Observable<any>((subscriber) => {
      // open hidden iframe to do login, user already authenticated before so it should redirect without any input needed
      const ele = document.createElement('iframe');
      ele.src = window.location.origin;
      ele.style.display = 'none';
      document.body.appendChild(ele);

      // fallback plan incase something go wrongs with cookie refresh: reload entire page
      const timeoutId = setTimeout(() => {
        this.authService.getState().pipe(take(1)).subscribe();
      }, 10000);

      window.addEventListener('message', (event) => {
        clearTimeout(timeoutId);
        
        try {
          // iframe already notifies the cookie in here so we remove it to avoid memory leak
          document.body.removeChild(ele);
        } catch (err) {
          console.error(err);
        }

        if (event.data == IFRAME_MESSAGE) {
          // resend the http call again, this time we have new cookie
          const newHttpRequest = originalHttpRequest.clone();

          this.httpClient.request(newHttpRequest).subscribe((res) => {
            subscriber.next(res);
          });
        }
      });
    });
  }
}
