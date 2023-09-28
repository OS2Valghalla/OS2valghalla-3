import { Injectable, isDevMode } from '@angular/core';
import { HttpClient, HttpBackend, HttpErrorResponse } from '@angular/common/http';
import { ReplaySubject, Observable, catchError, throwError, take } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { IFRAME_MESSAGE } from './consts';

const REDIRECT_URL = 'valghalla.redirectUrl';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly httpClient: HttpClient;

  readonly authorized = new ReplaySubject<boolean>(1);
  readonly authorized$ = this.authorized.asObservable();

  constructor(httpHandler: HttpBackend) {
    this.httpClient = new HttpClient(httpHandler);
  }

  getState(): Observable<void> {
    return new Observable<void>((observer) => {
      const apiPath = getBaseApiUrl() + 'auth/state';

      this.storeRedirectUrlIfNeeded();

      this.httpClient
        .get<string | null>(apiPath, {
          responseType: 'text' as any,
          withCredentials: isDevMode(),
        })
        .pipe(
          catchError((err: HttpErrorResponse) => {
            if (err.status == 401 || err.status == 403) {
              this.authorized.next(false);
              observer.next();
              observer.complete();
            }

            return throwError(() => err);
          }),
        )
        .subscribe((loginRedirectUrl?: string) => {
          if (loginRedirectUrl) {
            window.location.href = loginRedirectUrl;
            return;
          }

          // we're in an iframe to do silent cookie refresh
          if (window.location !== window.parent.location) {
            window.parent.postMessage(IFRAME_MESSAGE, '*');
            return;
          }

          this.authorized.next(true);
          this.redirectIfNeeded();

          observer.next();
          observer.complete();
        });
    });
  }

  logout() {
    this.httpClient
      .post<string>(getBaseApiUrl() + 'auth/logout', undefined, {
        responseType: 'text' as any,
        withCredentials: isDevMode(),
      })
      .pipe(
        take(1),
        catchError((err: HttpErrorResponse) => {
          return throwError(() => err);
        }),
      )
      .subscribe((redirectUrl) => {
        if (redirectUrl) {
          window.location.href = redirectUrl;
        }
      });
  }

  private storeRedirectUrlIfNeeded() {
    if (this.isRedirectRequired()) return;
    localStorage.setItem(REDIRECT_URL, window.location.href);
  }

  private redirectIfNeeded() {
    const urlParams = new URLSearchParams(window.location.search);
    const redirectRequired = urlParams.get('redirect');

    if (redirectRequired) {
      const redirectUrl = localStorage.getItem(REDIRECT_URL);

      if (redirectUrl) {
        window.location.href = redirectUrl;
      }
    }
  }

  private isRedirectRequired() {
    const urlParams = new URLSearchParams(window.location.search);
    return !!urlParams.get('redirect');
  }
}
