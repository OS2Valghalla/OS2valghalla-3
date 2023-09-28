import { Injectable } from '@angular/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { TranslocoService } from '@ngneat/transloco';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';

interface AuthorizeUrl {
  value: string;
}

@Injectable()
export class RegistrationHttpService {
  private baseUrl = getBaseApiUrl() + 'unprotected/registration/';

  private readonly httpClient: HttpClient;

  constructor(
    httpHandler: HttpBackend,
    private translocoService: TranslocoService,
  ) {
    this.httpClient = new HttpClient(httpHandler);
  }

  getAuthorizeUrl(registrationToken: string): Observable<AuthorizeUrl> {
    return this.httpClient
      .get<AuthorizeUrl>(this.baseUrl + 'getauthorizeurl', { params: { registrationToken: registrationToken } })
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'unprotected.error.registration.get_authorize_url',
                ),
              ),
          );
        }),
      );
  }
}
