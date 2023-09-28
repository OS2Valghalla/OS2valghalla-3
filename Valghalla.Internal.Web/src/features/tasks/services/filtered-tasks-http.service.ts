import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { TasksFiltersOptions } from '../models/tasks-filters-options';
import { AvailableTasksDetails } from '../models/available-tasks-details';
import { GetAvailableTasksByFiltersRequest } from '../models/get-available-tasks-by-filters-request';

@Injectable()
export class FilteredTasksHttpService {
  private baseUrl = getBaseApiUrl() + 'filteredtasks/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getTasksFiltersOptions(electionId: string): Observable<Response<TasksFiltersOptions>> {
    return this.httpClient
      .get<Response<TasksFiltersOptions>>(this.baseUrl + 'gettasksfiltersoptions', {
        params: {
          electionId: electionId,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.error.get_tasks_filtered_options');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  getAvailableTasksByFilters(
    value: GetAvailableTasksByFiltersRequest,
  ): Observable<Response<Array<AvailableTasksDetails>>> {
    return this.httpClient
      .post<Response<Array<AvailableTasksDetails>>>(this.baseUrl + 'getavailabletasksbyfilters', value)
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.error.link.create_filtered_tasks_link_failed');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {}),
      );
  }

  getParticipantsTasks(value): Observable<Response<Array<AvailableTasksDetails>>> {
    return this.httpClient
      .post<Response<Array<AvailableTasksDetails>>>(this.baseUrl + 'getparticipantstasks', value)
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.error.link.create_filtered_tasks_link_failed');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {}),
      );
  }

  auditLogExport(): Observable<void> {
    return this.httpClient.post<void>(this.baseUrl + 'auditlogexport', {});
  }
}
