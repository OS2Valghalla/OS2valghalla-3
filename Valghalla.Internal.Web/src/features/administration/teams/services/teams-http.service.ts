import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { Team } from '../models/team';
import { CreateTeamRequest } from '../models/create-team-request';
import { UpdateTeamRequest } from '../models/update-team-request';
import { NotificationService } from 'src/shared/services/notification.service';

@Injectable()
export class TeamHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/team/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getAllTeams(): Observable<Response<Array<Team>>> {
    return this.httpClient
      .get<Response<Array<Team>>>(this.baseUrl + 'getallteams')
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.teams.error.get_all_teams');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }


  getTeam(id: string): Observable<Response<Team>> {
    return this.httpClient
      .get<Response<Team>>(this.baseUrl + 'getteam', {
        params: {
          teamId: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.teams.error.get_team');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  createTeam(value: CreateTeamRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createteam', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.teams.error.create_team');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.teams.success.create_team');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateTeam(value: UpdateTeamRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updateteam', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.teams.error.update_team');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.teams.success.update_team');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  deleteTeam(id: string): Observable<Response<string>> {
    return this.httpClient
      .delete<Response<string>>(this.baseUrl + 'deleteteam', {
        params: {
          teamId: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.teams.error.delete_team');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.teams.success.delete_team');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }
}
