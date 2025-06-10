import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { TaskTypeDetails } from '../models/task-type-details';
import { TaskTypeListingItem } from '../models/task-type-listing-item';
import { CreateTaskTypeRequest } from '../models/create-task-type-request';
import { UpdateTaskTypeRequest } from '../models/update-task-type-request';

@Injectable()
export class TaskTypeHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/tasktype/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) { }

  getAllTaskTypes(): Observable<Response<Array<TaskTypeListingItem>>> {
    return this.httpClient
      .get<Response<Array<TaskTypeListingItem>>>(this.baseUrl + 'getalltasktypes', {
        params: {
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.task_type.error.get_all_task_types');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
  getTaskTypesByElectionID(electionId: string): Observable<Response<Array<TaskTypeListingItem>>> {
    return this.httpClient
      .get<Response<Array<TaskTypeListingItem>>>(this.baseUrl + 'getalltasktypesbyelectionid', {
        params: {
          electionId: electionId,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.task_type.error.get_all_task_types');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
  getTaskTypeDetails(id: string): Observable<Response<TaskTypeDetails>> {
    return this.httpClient
      .get<Response<TaskTypeDetails>>(this.baseUrl + 'gettasktypedetails', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.task_type.error.get_task_type_details');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  createTaskType(value: CreateTaskTypeRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createtasktype', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.task_type.error.create_task_type');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.task_type.success.create_task_type');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateTaskType(value: UpdateTaskTypeRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updatetasktype', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.task_type.error.update_task_type');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.task_type.success.update_task_type');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  deleteTaskType(id: string): Observable<Response<void>> {
    return this.httpClient
      .delete<Response<void>>(this.baseUrl + 'deletetasktype', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.task_type.error.delete_task_type');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.task_type.success.delete_task_type');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }
}
