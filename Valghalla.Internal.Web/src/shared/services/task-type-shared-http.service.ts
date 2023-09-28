import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../functions/url-helper';
import { Response } from '../models/respone';
import { TaskTypeShared } from '../models/task-type/task-type-shared';

@Injectable({
  providedIn: 'root',
})
export class TaskTypeSharedHttpService extends CacheQuery {
  static readonly GET_TASK_TYPES = 'TaskTypeSharedHttpService.getTaskTypes';

  private baseUrl = getBaseApiUrl() + 'shared/tasktype/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getTaskTypes(): Observable<Response<Array<TaskTypeShared>>> {
    return this.query(
      TaskTypeSharedHttpService.GET_TASK_TYPES,
      this.httpClient.get<Response<Array<TaskTypeShared>>>(this.baseUrl + 'gettasktypes').pipe(
        catchError(() => {
          return throwError(
            () => new Error(this.translocoService.translate('shared.error.get_task_types_shared')),
          );
        }),
        tap((res) => {
          if (!res.isSuccess) {
            this.invalidate(TaskTypeSharedHttpService.GET_TASK_TYPES);
          }
        }),
      ),
    );
  }
}
