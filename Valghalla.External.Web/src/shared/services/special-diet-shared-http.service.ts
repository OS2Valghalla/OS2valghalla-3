import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../functions/url-helper';
import { Response } from '../models/respone';
import { SpecialDietShared } from '../models/special-diet/special-diet-shared';

@Injectable({
  providedIn: 'root',
})
export class SpecialDietSharedHttpService extends CacheQuery {
  static readonly GET_SPECIAL_DIETS = 'SpecialDietSharedHttpService.getSpecialDiets';

  private baseUrl = getBaseApiUrl() + 'shared/specialdiet/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getSpecialDiets(): Observable<Response<Array<SpecialDietShared>>> {
    return this.query(
      SpecialDietSharedHttpService.GET_SPECIAL_DIETS,
      this.httpClient.get<Response<Array<SpecialDietShared>>>(this.baseUrl + 'getspecialdiets').pipe(
        catchError(() => {
          return throwError(() => new Error(this.translocoService.translate('shared.error.get_special_diets_shared')));
        }),
        tap((res) => {
          if (!res.isSuccess) {
            this.invalidate(SpecialDietSharedHttpService.GET_SPECIAL_DIETS);
          }
        }),
      ),
    );
  }
}
