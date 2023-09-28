import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../functions/url-helper';
import { Response } from '../models/respone';
import { WorkLocationShared } from '../models/work-location/work-location-shared';

@Injectable({
  providedIn: 'root',
})
export class WorkLocationSharedHttpService extends CacheQuery {
  static readonly GET_WORK_LOCATIONS = 'WorkLocationSharedHttpService.getWorkLocations';

  private baseUrl = getBaseApiUrl() + 'shared/worklocation/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getWorkLocations(): Observable<Response<Array<WorkLocationShared>>> {
    return this.query(
      WorkLocationSharedHttpService.GET_WORK_LOCATIONS,
      this.httpClient.get<Response<Array<WorkLocationShared>>>(this.baseUrl + 'getworklocations').pipe(
        catchError(() => {
          return throwError(() => new Error(this.translocoService.translate('shared.error.get_work_locations_shared')));
        }),
        tap((res) => {
          if (!res.isSuccess) {
            this.invalidate(WorkLocationSharedHttpService.GET_WORK_LOCATIONS);
          }
        }),
      ),
    );
  }
}
