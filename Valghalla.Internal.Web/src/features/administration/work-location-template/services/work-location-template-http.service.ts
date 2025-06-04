import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { CreateWorkLocationTemplateRequest } from '../models/create-work-location-template-request';
import { UpdateWorkLocationTemplateRequest } from '../models/update-work-location-template-request';
import { WorkLocationTemplateDetails } from '../models/work-location-template-details';

@Injectable()
export class WorkLocationTemplateHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/worklocationtemplate/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getWorkLocationTemplateDetails(id: string): Observable<Response<WorkLocationTemplateDetails>> {
    return this.httpClient
      .get<Response<WorkLocationTemplateDetails>>(this.baseUrl + 'getworklocationtemplate', {
        params: {
          workLocationTemplateId: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.work_location_template.error.get_work_location_template');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
getAllWorkLocationTemplates(): Observable<Response<WorkLocationTemplateDetails[]>> {
    return this.httpClient
      .get<Response<WorkLocationTemplateDetails[]>>(this.baseUrl + 'getworklocationtemplates', {        
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.work_location_template.error.get_work_location_template');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
  createWorkLocationTemplate(value: CreateWorkLocationTemplateRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createworklocationtemplate', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.work_location.error.create_work_location');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.work_location.success.create_work_location');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateWorkLocationTemplate(value: UpdateWorkLocationTemplateRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updateworklocationtemplate', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.work_location.error.update_work_location');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.work_location.success.update_work_location');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  deleteWorkLocationTemplate(id: string): Observable<Response<string>> {
    return this.httpClient
      .delete<Response<string>>(this.baseUrl + 'deleteworklocationtemplate', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.work_location.error.delete_work_location');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.work_location.success.delete_work_location');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }
}
