import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../functions/url-helper';
import { Response } from '../models/respone';
import { TeamShared } from '../models/team/team-shared';

@Injectable({
  providedIn: 'root',
})
export class TeamSharedHttpService extends CacheQuery {
  static readonly GET_TEAMS = 'TeamSharedHttpService.getTeams';

  private baseUrl = getBaseApiUrl() + 'shared/team/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getTeams(): Observable<Response<Array<TeamShared>>> {
    return this.query(
      TeamSharedHttpService.GET_TEAMS,
      this.httpClient.get<Response<Array<TeamShared>>>(this.baseUrl + 'getteams').pipe(
        catchError(() => {
          return throwError(() => new Error(this.translocoService.translate('shared.error.get_teams_shared')));
        }),
        tap((res) => {
          if (!res.isSuccess) {
            this.invalidate(TeamSharedHttpService.GET_TEAMS);
          }
        }),
      ),
    );
  }
}
