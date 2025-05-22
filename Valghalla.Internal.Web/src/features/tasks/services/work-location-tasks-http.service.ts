import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { WorkLocationInfo } from '../models/work-location-info';
import { WorkLocationTasksSummary } from '../models/work-location-tasks-summary';
import { TaskAssignment } from '../models/task-assignment';
import { AssignParticipantToTaskRequest } from '../models/assign-participant-to-task-request';
import { RemoveParticipantFromTaskRequest } from '../models/remove-participant-from-task-request';
import { ReplyForParticipantRequest } from '../models/reply-for-participant-request';
import { WorkLocationTasksDistributingRequest } from '../models/work-location-tasks-distributing-request';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { MoveTasksRequest } from '../models/move-tasks-request';

@Injectable()
export class WorkLocationTasksHttpService {
  private baseUrl = getBaseApiUrl() + 'worklocationtasks/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) { }

  getWorkLocation(workLocationId: string, electionId: string): Observable<Response<WorkLocationInfo>> {
    return this.httpClient
      .get<Response<WorkLocationInfo>>(getBaseApiUrl() + 'shared/worklocation/getworklocation', {
        params: {
          workLocationId: workLocationId,
          electionId: electionId
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.error.get_work_location');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  getWorkLocationTasksSummary(workLocationId: string, electionId: string): Observable<Response<WorkLocationTasksSummary>> {
    return this.httpClient
      .get<Response<WorkLocationTasksSummary>>(this.baseUrl + 'getworklocationtaskssummary', {
        params: {
          workLocationId: workLocationId,
          electionId: electionId
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.error.get_work_location_tasks_summary');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  distributeWorkLocationTasks(value: WorkLocationTasksDistributingRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'distributeworklocationtasks', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('tasks.error.distribute_work_location_tasks');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('tasks.success.distribute_work_location_tasks');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  getTaskAssignment(taskAssignmentId: string, electionId: string): Observable<Response<TaskAssignment>> {
    return this.httpClient
      .get<Response<TaskAssignment>>(this.baseUrl + 'gettaskassignment', {
        params: {
          taskAssignmentId: taskAssignmentId,
          electionId: electionId,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.error.get_task_assignment');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  getTeamTasks(teamId: string, workLocationId: string, electionId: string, isGettingRejectedTasks?: boolean): Observable<Response<Array<TaskAssignment>>> {
    return this.httpClient
      .get<Response<Array<TaskAssignment>>>(this.baseUrl + 'getteamtasks', {
        params: {
          teamId: teamId,
          workLocationId: workLocationId,
          electionId: electionId,
          isGettingRejectedTasks: isGettingRejectedTasks
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('tasks.error.get_team_tasks');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  assignParticipantToTask(value: AssignParticipantToTaskRequest): Observable<Response<boolean>> {
    return this.httpClient.post<Response<boolean>>(this.baseUrl + 'assignparticipanttotask', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('tasks.error.assign_participant_to_task');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('tasks.success.assign_participant_to_task');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  removeParticipantFromTask(value: RemoveParticipantFromTaskRequest): Observable<Response<void>> {
    return this.httpClient.post<Response<void>>(this.baseUrl + 'removeparticipantfromtask', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('tasks.error.remove_participant_from_task');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('tasks.success.remove_participant_from_task');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  replyForParticipant(value: ReplyForParticipantRequest): Observable<Response<void>> {
    return this.httpClient.post<Response<void>>(this.baseUrl + 'replyforparticipant', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('tasks.error.reply_for_participant');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('tasks.success.reply_for_participant');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }
  moveTasks(request: MoveTasksRequest): Observable<Response<void>> {
    return this.httpClient.post<Response<void>>(this.baseUrl + 'movetasks', request).pipe(
      catchError((err) => {
        this.notificationService.showSuccess(this.translocoService.translate('tasks.error.tasks_moved'));
        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          this.notificationService.showSuccess(this.translocoService.translate('tasks.success.tasks_moved'));          
        }
      }),
    );
  }
}