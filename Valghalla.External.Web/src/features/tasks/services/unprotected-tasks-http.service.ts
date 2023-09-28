import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { TasksFiltersOptions } from '../models/tasks-filters-options';
import { GetUnprotectedAvailableTasksByFiltersRequest } from '../models/get-unprotected-available-tasks-by-filters-request';
import { AvailableTasksDetails } from '../models/available-tasks-details';

@Injectable()
export class UnprotectedTasksHttpService {
  private baseUrl = getBaseApiUrl() + 'unprotected/tasks/';

  constructor(
    private httpClient: HttpClient, 
    private translocoService: TranslocoService,
    private notificationService: NotificationService,
    ) {
    
  }

  getTasksFiltersOptions(hashValue: string): Observable<Response<TasksFiltersOptions>> {
    return this.httpClient
      .get<Response<TasksFiltersOptions>>(this.baseUrl + 'gettasksfiltersoptions', {
        params: {
            hashValue: hashValue
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.error.get_tasks_filters_options');
          this.notificationService.showError(msg);     
          return throwError(() => err);
        }),
      );
  }

  getAvailableTasksByFilters(value: GetUnprotectedAvailableTasksByFiltersRequest): Observable<Response<Array<AvailableTasksDetails>>> {
    return this.httpClient
      .post<Response<Array<AvailableTasksDetails>>>(this.baseUrl + 'getavailabletasksbyfilters', value)
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.error.get_tasks');      
          this.notificationService.showError(msg);
          return throwError(() => err);
        }),
      );
  }
}
