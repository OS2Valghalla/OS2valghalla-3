import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { TasksSummary } from '../models/tasks-summary';
import { ElectionAreasGeneralInfo } from '../models/election-areas-general-info';

@Injectable()
export class AreaTasksHttpService {
  private baseUrl = getBaseApiUrl() + 'areatasks/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getAreasGeneralInfo(electionId: string): Observable<Response<ElectionAreasGeneralInfo>> {
    return this.httpClient
      .get<Response<ElectionAreasGeneralInfo>>(this.baseUrl + 'getareasgeneralinfo', {
        params: {
            electionId: electionId
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.error.get_areas_general_info');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  getAreaTasksSummary(electionId: string, selectedDate?: Date, selectedTeamId?: string): Observable<Response<Array<TasksSummary>>> {
    let params = new HttpParams();
    params = params.append('electionId', electionId);
    if (selectedDate) {
      params = params.append('selectedDate', selectedDate.toUTCString());
    }
    if (selectedTeamId) {
      params = params.append('selectedTeamId', selectedTeamId);
    }
    return this.httpClient
      .get<Response<Array<TasksSummary>>>(this.baseUrl + 'getareataskssummary', {
        params: params,
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.error.get_area_tasks_summary');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
}