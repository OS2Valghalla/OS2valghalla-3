import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { NotificationService } from 'src/shared/services/notification.service';
import { Response } from 'src/shared/models/respone';
import { WebPage } from 'src/shared/models/web-page';

@Injectable({
  providedIn: 'root',
})
export class WebHttpService {
  private baseUrl = getBaseApiUrl() + 'web/';

  constructor(
    private httpClient: HttpClient,
    private translocoService: TranslocoService,
    private notificationService: NotificationService,
  ) {}

  getDisclosureStatementPage(): Observable<Response<WebPage>> {
    return this.httpClient.get<Response<WebPage>>(this.baseUrl + 'disclosurestatement').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('shared.error.get_webpage_disclosurestatement');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  getDeclarationOfConsentPage(): Observable<Response<WebPage>> {
    return this.httpClient.get<Response<WebPage>>(this.baseUrl + 'declarationofconsent').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('shared.error.get_webpage_declarationofconsent');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }
}
