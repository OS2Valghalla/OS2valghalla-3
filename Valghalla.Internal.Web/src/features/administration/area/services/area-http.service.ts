import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { AreaDetails } from '../models/area-details';
import { AreaListingItem } from '../models/area-listing-item';
import { CreateAreaRequest } from '../models/create-area-request';
import { UpdateAreaRequest } from '../models/update-area-request';
import { NotificationService } from 'src/shared/services/notification.service';

@Injectable()
export class AreaHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/area/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getAllAreas(): Observable<Response<Array<AreaListingItem>>> {
    return this.httpClient.get<Response<Array<AreaListingItem>>>(this.baseUrl + 'getallareas').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.area.error.get_all_areas');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  getAreaDetails(id: string): Observable<Response<AreaDetails>> {
    return this.httpClient
      .get<Response<AreaDetails>>(this.baseUrl + 'getareadetails', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.area.error.get_area_details');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  createArea(value: CreateAreaRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createarea', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.area.error.create_area');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.area.success.create_area');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateArea(value: UpdateAreaRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updatearea', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.area.error.update_area');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.area.success.update_area');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  deleteArea(id: string): Observable<Response<void>> {
    return this.httpClient
      .delete<Response<void>>(this.baseUrl + 'deletearea', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.area.error.delete_area');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.area.success.delete_area');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }
}
