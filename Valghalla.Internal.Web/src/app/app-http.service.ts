import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { Observable, catchError, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { AppContext } from 'src/app/models/app-context';
import { CacheQuery } from 'src/shared/state/cache-query';
import { Response } from '../shared/models/respone';
import { AppElection } from './models/app-election';
import { NotificationService } from 'src/shared/services/notification.service';

@Injectable({
  providedIn: 'root',
})
export class AppHttpService extends CacheQuery {
  private baseUrl = getBaseApiUrl();

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {
    super();
  }

  getAppContext(electionId: string): Observable<Response<AppContext>> {
    const key = 'AppHttpService.getAppContext';
    return this.query(
      key,
      this.httpClient
        .get<Response<AppContext>>(
          this.baseUrl + 'app/context',
          electionId
            ? {
                params: {
                  electionId: electionId,
                },
              }
            : {},
        )
        .pipe(
          catchError(() => {
            return throwError(() => new Error(this.translocoService.translate('app.error.http.get_app_context')));
          }),
          tap((res) => {
            if (!res.isSuccess) {
              this.invalidate(key);
            }
          }),
        ),
    );
  }

  getElectionsToWorkOn(): Observable<Response<AppElection[]>> {
    return this.httpClient.get<Response<AppElection[]>>(this.baseUrl + 'app/getelectionstoworkon').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('app.error.http.get_elections_to_work_on');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }
}
