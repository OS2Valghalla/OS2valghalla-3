import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { WebPage } from 'src/shared/models/web-page';

@Injectable()
export class UnprotectedWebHttpService {
  private baseUrl = getBaseApiUrl() + 'unprotected/web/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getFrontPage(): Observable<Response<WebPage>> {
    return this.httpClient.get<Response<WebPage>>(this.baseUrl + 'front').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('shared.error.get_webpage_front');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  getFAQPage(): Observable<Response<WebPage>> {
    return this.httpClient.get<Response<WebPage>>(this.baseUrl + 'faq').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('shared.error.get_webpage_faq');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }
}