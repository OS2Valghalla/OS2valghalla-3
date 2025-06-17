import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { TaskTypeTemplateDetails } from '../models/task-type-template-details';
import { TaskTypeTemplateListingItem } from '../models/task-type-template-listing-item';
import { CreateTaskTypeTemplateRequest } from '../models/create-task-type-template-request';
import { UpdateTaskTypeTemplateRequest } from '../models/update-task-type-template-request';

@Injectable()
export class TaskTypeTemplateHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/tasktypetemplate/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) { }

  getAllTaskTypeTemplates(): Observable<Response<Array<TaskTypeTemplateDetails>>> {
    return this.httpClient
      .get<Response<Array<TaskTypeTemplateDetails>>>(this.baseUrl + 'getalltasktypetemplates', {
        params: {
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.task_type_template.error.get_all_task_type_templates');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  getTaskTypeTemplateDetails(id: string): Observable<Response<TaskTypeTemplateDetails>> {
    return this.httpClient
      .get<Response<TaskTypeTemplateDetails>>(this.baseUrl + 'gettasktypetemplatedetails', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.task_type_template.error.get_task_type_template_details');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  createTaskTypeTemplate(value: CreateTaskTypeTemplateRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createtasktypetemplate', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.task_type_template.error.create_task_type_template');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.task_type_template.success.create_task_type_template');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateTaskTypeTemplate(value: UpdateTaskTypeTemplateRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updatetasktypetemplate', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.task_type_template.error.update_task_type_template');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.task_type_template.success.update_task_type_template');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  deleteTaskTypeTemplate(id: string): Observable<Response<void>> {
    return this.httpClient
      .delete<Response<void>>(this.baseUrl + 'deletetasktypetemplate', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.task_type_template.error.delete_task_type_template');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.task_type_template.success.delete_task_type_template');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }
}
