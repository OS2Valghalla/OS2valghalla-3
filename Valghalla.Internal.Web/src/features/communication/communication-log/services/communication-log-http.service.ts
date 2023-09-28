import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { CommunicationLogDetails } from '../models/communication-log-details';

@Injectable()
export class CommunicationLogHttpService {
  private baseUrl = getBaseApiUrl() + 'communication/log/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getCommunicationLogDetails(id: string): Observable<Response<CommunicationLogDetails>> {
    return this.httpClient
      .get<Response<CommunicationLogDetails>>(this.baseUrl + 'getlogdetails', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate(
            'communication.communication_log.error.get_communication_log_details',
          );
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
}
