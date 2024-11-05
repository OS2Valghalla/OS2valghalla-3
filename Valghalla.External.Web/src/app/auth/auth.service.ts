import { Injectable, isDevMode } from '@angular/core';
import { HttpClient, HttpBackend, HttpErrorResponse } from '@angular/common/http';
import { ReplaySubject, Observable, catchError, throwError, of, EMPTY, take } from 'rxjs';
import { getLoginUrl, getLogoutUrl, getPingUrl, redirectIfNeeded, redirectToLoginIfNeeded, storeRedirectUrl } from './utils';

const SIGNED_IN_FLAG = 'ValghallaExternalTokenKey';
const UNAUTHORIZED_FLAG = '_unauthorized';

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
      return this.ping();
    }

    return EMPTY;
  }

  login(redirectUrl?: string) {
    console.log('login');
    
    storeRedirectUrl(redirectUrl);
    window.location.href = getLoginUrl();
  }

  logout(profileDeleted: boolean) {
    console.log('login');
    this.httpClient
      .post<string>(getLogoutUrl(), undefined, {
        params: {
          profileDeleted: profileDeleted,
        },
        responseType: 'text' as any,
        withCredentials: isDevMode(),
      })
      .pipe(
        take(1),
        catchError((err: HttpErrorResponse) => {
          if (redirectToLoginIfNeeded(err)) {
            return new Observable<any>();
          }

          return throwError(() => err);
        }),
      )
      .subscribe((redirectUrl) => {
        if (redirectUrl) {
          window.location.href = redirectUrl;
        }
      });
  }

  ping(): Observable<void> {
    return new Observable<void>((observer) => {
      this.httpClient
        .get<string | null>(getPingUrl(), {
          responseType: 'text' as any,
          withCredentials: isDevMode(),
        })
        .pipe(
          take(1),
          catchError((err: HttpErrorResponse) => {
            if (redirectIfNeeded()) {
              return of(UNAUTHORIZED_FLAG);
            }

            if (redirectToLoginIfNeeded(err)) {
              return new Observable<any>();
            }

            if (err.status == 401 || err.status == 403) {
              return of(UNAUTHORIZED_FLAG);
            }

            return throwError(() => err);
          }),
        )
        .subscribe((flag?: string) => {
          if (flag == UNAUTHORIZED_FLAG) {
            this.authorized.next(false);
          }
          else {
            this.authorized.next(true);
            redirectIfNeeded();
          }

          observer.next();
          observer.complete();
        });
    });
  }

  isSessionExist() {
    const value = document.cookie.match('(^|;)\\s*' + SIGNED_IN_FLAG + '\\s*=\\s*([^;]+)')?.pop();
    return !!value;
  }
}
