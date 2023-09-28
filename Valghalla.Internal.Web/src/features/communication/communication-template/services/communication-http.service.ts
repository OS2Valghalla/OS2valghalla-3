import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { CommunicationTemplateDetails } from '../models/communication-template-details';
import { CreateCommunicationTemplateRequest } from '../models/create-communication-template-request';
import { UpdateCommunicationTemplateRequest } from '../models/update-communication-template-request';

@Injectable()
export class CommunicationHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/communication/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getElectionCommunicationTemplate(id: string): Observable<Response<CommunicationTemplateDetails>> {
    return this.httpClient
      .get<Response<CommunicationTemplateDetails>>(this.baseUrl + 'getcommunicationtemplate', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('communication.error.get_communication_template');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  getParticipantsForSendingGroupMessage(value): Observable<Response<any>> {
    return this.httpClient
      .post<Response<any>>(this.baseUrl + 'getparticipantsforsendinggroupmessage', value)
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('communication.error.get_participants');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  sendGroupMessage(value): Observable<Response<any>> {
    return this.httpClient
      .post<Response<any>>(this.baseUrl + 'sendgroupmessage', value)
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('communication.error.send_group_message');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('communication.success.send_group_message');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  createCommunicationTemplate(value: CreateCommunicationTemplateRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createcommunicationtemplate', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('communication.error.create_communication_template');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('communication.success.create_communication_template');
          this.notificationService.showSuccess(msg);
        }
      }),
    );    
  }

  updateCommunicationTemplate(value: UpdateCommunicationTemplateRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updatecommunicationtemplate', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('communication.error.update_communication_template');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('communication.success.update_communication_template');
          this.notificationService.showSuccess(msg);
        }
      }),
    );    
  }

  deleteCommunicationTemplate(id: string): Observable<Response<void>> {
    return this.httpClient
      .delete<Response<void>>(this.baseUrl + 'deletecommunicationtemplate', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('communication.error.delete_communication_template');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('communication.success.delete_communication_template');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }
}