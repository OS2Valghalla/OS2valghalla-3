import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../functions/url-helper';
import { Response } from '../models/respone';
import { ElectionShared } from '../models/election/election-shared';

@Injectable({
  providedIn: 'root',
})
export class ElectionSharedHttpService extends CacheQuery {
  static readonly GET_ELECTIONS = 'ElectionSharedHttpService.getElections';

  private baseUrl = getBaseApiUrl() + 'shared/election/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getElections(): Observable<Response<Array<ElectionShared>>> {
    return this.query(
      ElectionSharedHttpService.GET_ELECTIONS,
      this.httpClient.get<Response<Array<ElectionShared>>>(this.baseUrl + 'getelections').pipe(
        catchError(() => {
          return throwError(
            () => new Error(this.translocoService.translate('shared.error.get_elections_shared')),
          );
        }),
        tap((res) => {
          if (!res.isSuccess) {
            this.invalidate(ElectionSharedHttpService.GET_ELECTIONS);
          }
        }),
      ),
    );
  }
}
