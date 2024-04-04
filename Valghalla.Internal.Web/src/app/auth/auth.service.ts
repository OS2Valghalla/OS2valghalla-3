import { Injectable, isDevMode } from '@angular/core';
import { HttpClient, HttpBackend, HttpErrorResponse } from '@angular/common/http';
import { ReplaySubject, Observable, catchError, throwError, take } from 'rxjs';
import { getLogoutUrl, getPingUrl, redirectIfNeeded, redirectToLoginIfNeeded } from './utils';

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

  ping(): Observable<void> {
    return new Observable<void>((observer) => {
      this.httpClient
        .get<string | null>(getPingUrl(), {
          responseType: 'text' as any,
          withCredentials: isDevMode(),
        })
        .pipe(
          catchError((err: HttpErrorResponse) => {
            if (redirectToLoginIfNeeded(err)) {
              return new Observable<any>();
            }

            if (err.status == 401 || err.status == 403) {
              this.authorized.next(false);
              observer.next();
              observer.complete();
            }

            return throwError(() => err);
          }),
        )
        .subscribe(() => {
          this.authorized.next(true);
          redirectIfNeeded();

          observer.next();
          observer.complete();
        });
    });
  }

  logout() {
    this.httpClient
      .post<string>(getLogoutUrl(), undefined, {
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
}
