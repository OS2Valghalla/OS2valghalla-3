import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { MyProfile } from '../models/my-profile';
import { UpdateMyProfileRequest } from '../models/update-my-profile-request';
import { MyProfilePermission } from '../models/my-profile-permission';

@Injectable()
export class MyProfileHttpService {
  private baseUrl = getBaseApiUrl() + 'myprofile/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getMyProfile(): Observable<Response<MyProfile>> {
    return this.httpClient.get<Response<MyProfile>>(this.baseUrl + 'getmyprofile').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('my_profile.error.get_my_profile');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  getMyProfilePermission(): Observable<Response<MyProfilePermission>> {
    return this.httpClient.get<Response<MyProfilePermission>>(this.baseUrl + 'getmyprofilepermission').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('my_profile.error.get_my_profile_permission');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  updateMyProfile(value: UpdateMyProfileRequest): Observable<Response<void>> {
    return this.httpClient.post<Response<void>>(this.baseUrl + 'updatemyprofile', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('my_profile.error.update_my_profile');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('my_profile.success.update_my_profile');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  deleteMyProfile(): Observable<Response<void>> {
    return this.httpClient.delete<Response<void>>(this.baseUrl + 'deletemyprofile').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('my_profile.error.delete_my_profile');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('my_profile.success.delete_my_profile');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }
}
