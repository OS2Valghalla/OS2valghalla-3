import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { TeamResponsibleTasks, TeamResponsibleTasksFiltersOptions } from '../models/team-responsible-tasks';
import { TaskPreview } from '../models/task-preview';
import { GetTeamResponsibleTasksRequest } from '../models/get-team-responsible-tasks-request';
import { NotificationService } from 'src/shared/services/notification.service';

@Injectable()
export class TeamResponsibleTaskHttpService {
  private baseUrl = getBaseApiUrl() + 'teamResponsibleTasks/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getTeamResponsibleTasksFiltersOptions(): Observable<Response<TeamResponsibleTasksFiltersOptions>> {
    return this.httpClient
      .get<Response<TeamResponsibleTasksFiltersOptions>>(this.baseUrl + 'getteamresponsibletasksfiltersoptions')
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('task_distribution.error.get_team_responsible_tasks_filters_options');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  getTeamResponsibleTasks(request: GetTeamResponsibleTasksRequest): Observable<Response<TeamResponsibleTasks>> {
    return this.httpClient
      .post<Response<TeamResponsibleTasks>>(this.baseUrl + 'getteamresponsibletasks', request)
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('task_distribution.error.get_team_responsible_tasks');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  getTeamResponsibleTaskPreview(hashValue: string, invitationCode?: string): Observable<Response<TaskPreview>> {
    let query = {
      hashValue: hashValue,
    };

    if (invitationCode) {
      query = {
        ...query,
        ...{
          code: invitationCode,
        },
      };
    }

    return this.httpClient
      .get<Response<TaskPreview>>(this.baseUrl + 'getteamresponsibletaskpreview', {
        params: query,
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.task_preview.error.get_task_preview');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
}
