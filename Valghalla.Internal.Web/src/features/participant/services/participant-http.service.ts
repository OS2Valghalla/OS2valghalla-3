import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { CreateParticipantRequest } from '../models/create-participant-request';
import { ParticipantDetails } from '../models/participant-details';
import { ParticipantPersonalRecord } from '../models/participant-personal-record';
import { UpdateParticipantRequest } from '../models/update-participant-request';
import { CreateParticipantEventLogRequest } from '../models/create-participant-event-log-request';
import { UpdateParticipantEventLogRequest } from '../models/update-participant-event-log-request';
import { ParticipantTask } from '../models/participant-task';

@Injectable()
export class ParticipantHttpService {
  private baseUrl = getBaseApiUrl() + 'participant/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getParticipantDetails(id: string): Observable<Response<ParticipantDetails>> {
    return this.httpClient
      .get<Response<ParticipantDetails>>(this.baseUrl + 'getparticipantdetails', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('participant.error.get_participant_details');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  getParticipantPersonalRecord(cpr: string): Observable<Response<ParticipantPersonalRecord>> {
    return this.httpClient
      .get<Response<ParticipantPersonalRecord>>(this.baseUrl + 'getparticipantpersonalrecord', {
        params: {
          cpr: cpr,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('participant.error.get_participant_personal_record');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  createParticipant(value: CreateParticipantRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createparticipant', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('participant.error.create_participant');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('participant.success.create_participant');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateParticipant(value: UpdateParticipantRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updateparticipant', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('participant.error.update_participant');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('participant.success.update_participant');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  deleteParticipant(id: string): Observable<Response<void>> {
    return this.httpClient
      .delete<Response<void>>(this.baseUrl + 'deleteparticipant', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('participant.error.delete_participant');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('participant.success.delete_participant');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  getTeamResponsibleRights(id: string): Observable<Response<string[]>> {
    return this.httpClient
      .get<Response<string[]>>(this.baseUrl + 'getteamresponsiblerights', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('participant.error.get_team_responsible_rights');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  createParticipantEventLog(value: CreateParticipantEventLogRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createparticipanteventlog', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('participant.error.create_participant_event_log');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('participant.success.create_participant_event_log');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateParticipantEventLog(value: UpdateParticipantEventLogRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updateparticipanteventlog', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('participant.error.update_participant_event_log');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('participant.success.update_participant_event_log');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  deleteParticipantEventLog(id: string): Observable<Response<void>> {
    return this.httpClient
      .delete<Response<void>>(this.baseUrl + 'deleteparticipanteventlog', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('participant.error.delete_participant_event_log');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('participant.success.delete_participant_event_log');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  getWorkLocationResponsibleRights(id: string): Observable<Response<string[]>> {
    return this.httpClient
      .get<Response<string[]>>(this.baseUrl + 'getworklocationresponsiblerights', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('participant.error.get_work_location_responsible_rights');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  bulkDeleteParticipants(values: string[]): Observable<Response<void>> {
    return this.httpClient.post<Response<void>>(this.baseUrl + 'bulkdeleteparticipants', values).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('participant.error.bulk_delete_participants');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('participant.success.bulk_delete_participants');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  getParticipantTasks(id: string): Observable<Response<ParticipantTask[]>> {
    return this.httpClient
      .get<Response<ParticipantTask[]>>(this.baseUrl + 'getparticipanttasks', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('participant.error.get_participant_tasks');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
}
