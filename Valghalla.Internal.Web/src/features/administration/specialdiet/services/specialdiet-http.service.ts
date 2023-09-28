import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { SpecialDiet } from '../models/specialdiet';
import { CreateSpecialDietRequest } from '../models/create-specialdiet-request';
import { UpdateSpecialDietRequest } from '../models/update-specialdiet-request';
import { NotificationService } from 'src/shared/services/notification.service';

@Injectable()
export class SpecialDietHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/specialdiet/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getSpecialDiet(id: string): Observable<Response<SpecialDiet>> {
    return this.httpClient
      .get<Response<SpecialDiet>>(this.baseUrl + 'getspecialdiet', {
        params: {
          specialDietId: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.specialdiet.error.get_specialdiet');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  createSpecialDiet(value: CreateSpecialDietRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createspecialdiet', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.specialdiet.error.create_specialdiet');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.specialdiet.success.create_specialdiet');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateSpecialDiet(value: UpdateSpecialDietRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updatespecialdiet', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.specialdiet.error.update_specialdiet');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.specialdiet.success.update_specialdiet');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  deleteSpecialDiet(id: string): Observable<Response<string>> {
    return this.httpClient
      .delete<Response<string>>(this.baseUrl + 'deletespecialdiet', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.specialdiet.error.delete_specialdiet');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.specialdiet.success.delete_specialdiet');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }
}
