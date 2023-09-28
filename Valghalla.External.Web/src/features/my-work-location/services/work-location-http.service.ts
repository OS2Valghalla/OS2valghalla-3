import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError, tap } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { WorkLocationShared } from 'src/shared/models/work-location-shared';
import { WorkLocationTasksDetails } from '../models/work-location-details';

@Injectable()
export class WorkLocationHttpService {
  private baseUrl = getBaseApiUrl() + 'worklocation/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getMyTeams(): Observable<Response<Array<WorkLocationShared>>> {
    return this.httpClient.get<Response<Array<WorkLocationShared>>>(this.baseUrl + 'getmyworklocations').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('my_work_location.error.get_my_work_locations');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  getWorkLocationDates(workLocationId: string): Observable<Response<Array<Date>>> {
    return this.httpClient.get<Response<Array<Date>>>(this.baseUrl + 'getworklocationdates', { params: {
      workLocationId: workLocationId
    }}).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('my_work_location.error.get_work_location_dates');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  getWorkLocationDetails(workLocationId: string, taskDate: string): Observable<Response<WorkLocationTasksDetails>> {
    return this.httpClient.get<Response<WorkLocationTasksDetails>>(this.baseUrl + 'getworklocationdetails', { params: {
      workLocationId: workLocationId,
      taskDate: taskDate
    }}).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('my_work_location.error.get_work_location_details');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }
}
