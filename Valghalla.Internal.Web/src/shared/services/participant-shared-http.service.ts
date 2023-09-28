import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../functions/url-helper';
import { Response } from '../models/respone';
import { ParticipantShared } from '../models/participant/participant-shared';

@Injectable({
  providedIn: 'root',
})
export class ParticipantSharedHttpService extends CacheQuery {
  private baseUrl = getBaseApiUrl() + 'shared/participant/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getParticipants(values: string[]): Observable<Response<Array<ParticipantShared>>> {
    return this.httpClient.post<Response<Array<ParticipantShared>>>(this.baseUrl + 'getparticipants', values).pipe(
      catchError(() => {
        return throwError(() => new Error(this.translocoService.translate('shared.error.get_participants_shared')));
      }),
    );
  }
}
