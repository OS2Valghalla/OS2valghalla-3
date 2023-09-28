import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError, tap } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { Team } from '../models/team';
import { TeamMember } from '../models/team-member';

@Injectable()
export class TeamHttpService {
  private baseUrl = getBaseApiUrl() + 'team/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getMyTeams(): Observable<Response<Array<Team>>> {
    return this.httpClient.get<Response<Array<Team>>>(this.baseUrl + 'getmyteams').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('my_team.error.get_my_teams');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  getTeamMembers(teamId: string): Observable<Response<Array<TeamMember>>> {
    return this.httpClient
      .get<Response<Array<TeamMember>>>(this.baseUrl + 'getteammembers', {
        params: {
          teamId: teamId,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('my_team.error.get_team_members');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  removeTeamMember(teamId: string, participantId: string): Observable<Response<void>> {
    return this.httpClient
      .post<Response<void>>(this.baseUrl + 'removemember', {
        teamId: teamId,
        participantId: participantId,
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('my_team.error.remove_member');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('my_team.success.remove_member');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  createTeamLink(teamId: string): Observable<Response<string>> {
    return this.httpClient
      .post<Response<string>>(this.baseUrl + 'createteamlink', {
        teamId: teamId,
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('my_team.error.create_team_link');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
}
