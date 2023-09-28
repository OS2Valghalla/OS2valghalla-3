import { Injectable, isDevMode } from '@angular/core';
import { HttpClient, HttpBackend, HttpErrorResponse } from '@angular/common/http';
import { ReplaySubject, Observable, catchError, throwError, of, EMPTY, take } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { IFRAME_MESSAGE } from './consts';

const REDIRECT_URL = 'valghalla.redirectUrl';
const COOKIE_STATE = 'valghalla.signedin';
const IGNORE_CODE = '__ignore';

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

  initialize() {
    if (this.isSessionExist()) {
      return this.getState();
    }

    return EMPTY;
  }

  login(redirectUrl?: string) {
    const apiPath = getBaseApiUrl() + 'auth/login';
    this.storeRedirectUrl(redirectUrl);
    window.location.href = apiPath;
  }

  logout(profileDeleted: boolean) {
    this.httpClient
      .post<string>(getBaseApiUrl() + 'auth/logout', undefined, {
        params: {
          profileDeleted: profileDeleted,
        },
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

  getState(): Observable<void> {
    return new Observable<void>((observer) => {
      const apiPath = getBaseApiUrl() + 'auth/state';

      if (!this.isRedirectRequired()) {
        this.storeRedirectUrl();
      }

      this.httpClient
        .get<string | null>(apiPath, {
          responseType: 'text' as any,
          withCredentials: isDevMode(),
        })
        .pipe(
          take(1),
          catchError((err: HttpErrorResponse) => {
            if (err.status == 401 || err.status == 403) {
              this.authorized.next(false);
              this.redirectIfNeeded();

              observer.next();
              observer.complete();
              return of(IGNORE_CODE);
            }

            return throwError(() => err);
          }),
        )
        .subscribe((loginRedirectUrl?: string) => {
          if (loginRedirectUrl == IGNORE_CODE) return;

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

  isSessionExist() {
    const value = document.cookie.match('(^|;)\\s*' + COOKIE_STATE + '\\s*=\\s*([^;]+)')?.pop();
    return !!value;
  }

  private storeRedirectUrl(url?: string) {
    if (this.isRedirectRequired()) return;
    localStorage.setItem(REDIRECT_URL, url ?? window.location.href);
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
