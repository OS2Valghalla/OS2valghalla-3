import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../functions/url-helper';
import { Response } from '../models/respone';
import { CommunicationTemplateShared } from '../models/communication/communication-template-shared';

@Injectable({
  providedIn: 'root',
})
export class CommunicationSharedHttpService extends CacheQuery {
  static readonly GET_COMMUNICATION_TEMPLATES = 'CommunicationSharedHttpService.getElections';

  private baseUrl = getBaseApiUrl() + 'shared/communication/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getCommunicationTemplates(): Observable<Response<Array<CommunicationTemplateShared>>> {
    return this.query(
      CommunicationSharedHttpService.GET_COMMUNICATION_TEMPLATES,
      this.httpClient.get<Response<Array<CommunicationTemplateShared>>>(this.baseUrl + 'getcommunicationtemplates').pipe(
        catchError(() => {
          return throwError(
            () => new Error(this.translocoService.translate('shared.error.get_communication_templates_shared')),
          );
        }),
        tap((res) => {
          if (!res.isSuccess) {
            this.invalidate(CommunicationSharedHttpService.GET_COMMUNICATION_TEMPLATES);
          }
        }),
      ),
    );
  }
}
