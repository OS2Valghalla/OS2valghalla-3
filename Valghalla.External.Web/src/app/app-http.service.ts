import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { CacheQuery } from 'src/shared/state/cache-query';
import { Response } from 'src/shared/models/respone';
import { AppContext } from 'src/app/models/app-context';
import { UserTeam } from './models/user-team';

@Injectable({
  providedIn: 'root',
})
export class AppHttpService extends CacheQuery {
  private baseUrl = getBaseApiUrl();

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getAppContext(): Observable<Response<AppContext>> {
    const key = 'AppHttpService.getAppContext';
    return this.query(
      key,
      this.httpClient.get<Response<AppContext>>(this.baseUrl + 'app/context').pipe(
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

  getUserTeams(): Observable<Response<UserTeam[]>> {
    return this.httpClient.get<Response<UserTeam[]>>(this.baseUrl + 'app/userteams').pipe(
      catchError(() => {
        return throwError(() => new Error(this.translocoService.translate('app.error.http.get_user_teams')));
      }),
    );
  }
}
