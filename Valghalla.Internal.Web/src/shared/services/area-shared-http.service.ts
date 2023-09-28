import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../functions/url-helper';
import { Response } from '../models/respone';
import { AreaShared } from '../models/area/area-shared';

@Injectable({
  providedIn: 'root',
})
export class AreaSharedHttpService extends CacheQuery {
  static readonly GET_AREAS = 'AreaSharedHttpService.getAreas';

  private baseUrl = getBaseApiUrl() + 'shared/area/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getAreas(): Observable<Response<Array<AreaShared>>> {
    return this.query(
      AreaSharedHttpService.GET_AREAS,
      this.httpClient.get<Response<Array<AreaShared>>>(this.baseUrl + 'getareas').pipe(
        catchError(() => {
          return throwError(
            () => new Error(this.translocoService.translate('shared.error.get_areas_shared')),
          );
        }),
        tap((res) => {
          if (!res.isSuccess) {
            this.invalidate(AreaSharedHttpService.GET_AREAS);
          }
        }),
      ),
    );
  }
}
