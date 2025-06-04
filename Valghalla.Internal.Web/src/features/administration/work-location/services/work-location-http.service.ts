import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { CreateWorkLocationRequest } from '../models/create-work-location-request';
import { UpdateWorkLocationRequest } from '../models/update-work-location-request';
import { WorkLocationDetails } from '../models/work-location-details';

@Injectable()
export class WorkLocationHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/worklocation/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) { }

  getWorkLocationDetails(id: string): Observable<Response<WorkLocationDetails>> {
    return this.httpClient
      .get<Response<WorkLocationDetails>>(this.baseUrl + 'getworklocation', {
        params: {
          workLocationId: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.work_location.error.get_work_location');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
  getWorkLocationByElectionId(workLocationId: string, electionId: string): Observable<Response<WorkLocationDetails>> {
    return this.httpClient
      .get<Response<WorkLocationDetails>>(this.baseUrl + 'getworklocationbyelectionid', {
        params: {
          workLocationId: workLocationId,
          electionId: electionId,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.work_location.error.get_work_location');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
  createWorkLocation(value: CreateWorkLocationRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createworklocation', value).pipe(
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

  updateWorkLocation(value: UpdateWorkLocationRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updateworklocation', value).pipe(
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

  deleteWorkLocation(id: string): Observable<Response<string>> {
    return this.httpClient
      .delete<Response<string>>(this.baseUrl + 'deleteworklocation', {
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
