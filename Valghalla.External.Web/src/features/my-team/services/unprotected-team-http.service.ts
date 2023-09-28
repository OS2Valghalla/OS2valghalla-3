import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { Team } from '../models/team';
@Injectable()
export class UnprotectedTeamHttpService {
  private baseUrl = getBaseApiUrl() + 'unprotected/team/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getTeamByTeamLink(hashValue: string): Observable<Response<Team>> {
    return this.httpClient
      .get<Response<Team>>(this.baseUrl + 'getteambyteamlink', {
        params: {
            hashValue: hashValue
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('my_team.error.get_team_by_team_link');
          this.notificationService.showError(msg);     
          return throwError(() => err);
        }),
      );
  }
}