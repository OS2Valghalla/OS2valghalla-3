import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { QueryForm } from './models/query-form';
import { catchError, map, Observable, throwError } from 'rxjs';
import { Response } from '../models/respone';
import { QueryResult } from './models/query-result';
import { QueryFormInfo } from './models/query-form-info';
import { TranslocoService } from '@ngneat/transloco';

@Injectable({
  providedIn: 'root',
})
export class QueryEngineClient<
  TQueryForm extends QueryForm,
  TQueryResultItem,
  TQueryFormParameters,
> {
  private baseUrl = getBaseApiUrl();

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
  ) {}

  query(
    path: string,
    form: TQueryForm,
  ): Observable<Response<QueryResult<TQueryResultItem>>> {
    return this.httpClient
      .post<Response<QueryResult<TQueryResultItem>>>(this.baseUrl + path, form, {withCredentials: true})
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'shared.query_engine.error.query',
                ) + ` (${path})`,
              ),
          );
        }),
      );
  }

  getQueryFormInfo(
    path: string,
    params: TQueryFormParameters,
  ): Observable<Response<QueryFormInfo<TQueryForm>>> {
    return this.httpClient
      .post<Response<QueryFormInfo<TQueryForm>>>(this.baseUrl + path, params)
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'shared.query_engine.error.get_query_form',
                ) + ` (${path})`,
              ),
          );
        }),
        map((res) => {
          if (res.isSuccess) {
            res.data.filters = JSON.parse(res.data.filters as any);
            res.data.properties = JSON.parse(res.data.properties as any);
          }

          return res;
        }),
      );
  }
}
