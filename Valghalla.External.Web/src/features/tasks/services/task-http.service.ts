import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { TaskPreview, TaskDetails } from '../models/task-preview';
import { NotificationService } from 'src/shared/services/notification.service';
import { TaskConfirmationResult } from '../models/task-confirmation-result';
import { TaskOverviewFilterOptions } from '../models/task-overview-filter-options';
import { GetTaskOverviewRequest } from '../models/get-task-overview-request';
import { TaskOverviewItem } from '../models/task-overview-item';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { Router } from '@angular/router';

const ALIVE_RULE = '0cfc8f2f-d09f-4776-9e27-6ecf7fdd668d';
const AGE_RULE = '78dca41a-0a59-445d-b639-de22ea7e5569';
const MUNICIPALITY_RULE = 'df6094f4-26b5-48f3-9d49-7508537230a2';
const CITIZENSHIP_RULE = '4ac40dc3-2843-402c-88d9-a09f73e5ee8c';
const LEGAL_ADULT_RULE = '6a4d315e-e459-4e87-81eb-2339dbe5df5b';

@Injectable()
export class TaskHttpService {
  private baseUrl = getBaseApiUrl() + 'tasks/';

  constructor(
    private readonly router: Router,
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) { }

  getTaskPreview(hashValue: string, invitationCode?: string): Observable<Response<TaskPreview>> {
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
      .get<Response<TaskPreview>>(this.baseUrl + 'gettaskpreview', {
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

  getMyTasks(): Observable<Response<Array<TaskDetails>>> {
    return this.httpClient.get<Response<Array<TaskDetails>>>(this.baseUrl + 'getmytasks').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('tasks.my_tasks.error.get_my_tasks');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  getDownloadFileLink(id: string) {
    return this.baseUrl + 'download?id=' + id;
  }

  acceptTask(
    hashValue: string,
    invitationCode?: string,
    taskInvitation?: boolean,
    showStepIndicator?: boolean,
  ): Observable<Response<TaskConfirmationResult>> {
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
    if (taskInvitation) {
      query = {
        ...query,
        ...{
          taskInvitation: taskInvitation,
        },
      };
    }

    return this.httpClient
      .post<Response<TaskConfirmationResult>>(this.baseUrl + 'accepttask', undefined, {
        params: query,
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.task_invitation.error.accept_task');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (!res.isSuccess) return;

          let path: string[] = [];

          if (res.data.succeed) {
            path = [RoutingNodes.Tasks, RoutingNodes.TaskAcceptanceConfirmation, hashValue];
          } else if (res.data.cprInvalid) {
            path = [RoutingNodes.Tasks, RoutingNodes.TaskAcceptanceCprInvalid];
          } else if (res.data.conflicted) {
            path = [RoutingNodes.Tasks, RoutingNodes.TaskAcceptanceConflict];
          } else if (res.data.failedRuleIds.length > 0) {
            const rules = [] as number[];

            if (res.data.failedRuleIds.includes(ALIVE_RULE)) {
              rules.push(0);
            }

            if (res.data.failedRuleIds.includes(AGE_RULE)) {
              rules.push(1);
            }

            if (res.data.failedRuleIds.includes(MUNICIPALITY_RULE)) {
              rules.push(2);
            }

            if (res.data.failedRuleIds.includes(CITIZENSHIP_RULE)) {
              rules.push(3);
            }

            if (res.data.failedRuleIds.includes(LEGAL_ADULT_RULE)) {
              rules.push(4);
            }

            path = [
              RoutingNodes.Tasks,
              RoutingNodes.TaskAcceptanceValidationFailure,
              encodeURIComponent(JSON.stringify(rules)),
            ];
          }

          if (showStepIndicator) {
            window.location.href = [window.location.origin, ...path].join('/') + '?stepper=true';
          } else {
            this.router.navigate(path);
          }
        }),
      );
  }

  rejectTask(hashValue: string, invitationCode?: string): Observable<Response<TaskConfirmationResult>> {
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
      .post<Response<TaskConfirmationResult>>(this.baseUrl + 'rejecttask', undefined, {
        params: query,
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.task_invitation.error.reject_task');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (!res.isSuccess) return;

          let path: string[] = [];

          if (res.data.succeed) {
            path = [RoutingNodes.Tasks, RoutingNodes.TaskRejectionConfirmation, hashValue];
          } else if (res.data.cprInvalid) {
            path = [RoutingNodes.Tasks, RoutingNodes.TaskAcceptanceCprInvalid];
          }

          this.router.navigate(path);
        }),
      );
  }

  unregisterTask(taskAssignmentId: string, hashValue: string): Observable<Response<TaskConfirmationResult>> {
    return this.httpClient
      .post<Response<TaskConfirmationResult>>(this.baseUrl + 'unregistertask', undefined, {
        params: {
          taskAssignmentId: taskAssignmentId,
          hashValue: hashValue,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.task_invitation.error.unregister_task');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('tasks.task_invitation.success.unregister_task');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  getTaskOverviewFilterOptions(): Observable<Response<TaskOverviewFilterOptions>> {
    return this.httpClient.get<Response<TaskOverviewFilterOptions>>(this.baseUrl + 'gettaskoverviewfilteroptions').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('tasks.landing.error.get_task_overview_filter_options');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  getTaskOverview(value: GetTaskOverviewRequest): Observable<Response<TaskOverviewItem[]>> {
    return this.httpClient.post<Response<TaskOverviewItem[]>>(this.baseUrl + 'gettaskoverview', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('tasks.landing.error.get_task_overview');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }
}
