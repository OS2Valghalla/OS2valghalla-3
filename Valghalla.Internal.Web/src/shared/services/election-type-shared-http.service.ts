import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../functions/url-helper';
import { Response } from '../models/respone';
import { ElectionTypeShared } from '../models/election-type/election-type-shared';

@Injectable({
  providedIn: 'root',
})
export class ElectionTypeSharedHttpService extends CacheQuery {
  static readonly GET_ELECTION_TYPES = 'ElectionTypeSharedHttpService.getElectionTypes';

  private baseUrl = getBaseApiUrl() + 'shared/electiontype/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getElectionTypes(): Observable<Response<Array<ElectionTypeShared>>> {
    return this.query(
      ElectionTypeSharedHttpService.GET_ELECTION_TYPES,
      this.httpClient.get<Response<Array<ElectionTypeShared>>>(this.baseUrl + 'getelectiontypes').pipe(
        catchError(() => {
          return throwError(
            () => new Error(this.translocoService.translate('shared.error.get_election_types_shared')),
          );
        }),
        tap((res) => {
          if (!res.isSuccess) {
            this.invalidate(ElectionTypeSharedHttpService.GET_ELECTION_TYPES);
          }
        }),
      ),
    );
  }
}
