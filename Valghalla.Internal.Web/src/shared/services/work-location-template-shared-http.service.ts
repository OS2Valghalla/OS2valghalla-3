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
export class WorkLocationTemplateSharedHttpService extends CacheQuery {
  static readonly GET_WORK_LOCATIONS = 'WorkLocationTemplateSharedHttpService.getWorkLocationtemplates';

  private baseUrl = getBaseApiUrl() + 'shared/worklocationtemplate/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getWorkLocationTemplates(): Observable<Response<Array<WorkLocationShared>>> {
    return this.query(
      WorkLocationTemplateSharedHttpService.GET_WORK_LOCATIONS,
      this.httpClient.get<Response<Array<WorkLocationShared>>>(this.baseUrl + 'getworklocationtemplates').pipe(
        catchError(() => {
          return throwError(() => new Error(this.translocoService.translate('shared.error.get_work_locations_template_shared')));
        }),
        tap((res) => {
          if (!res.isSuccess) {
            this.invalidate(WorkLocationTemplateSharedHttpService.GET_WORK_LOCATIONS);
          }
        }),
      ),
    );
  }
}
