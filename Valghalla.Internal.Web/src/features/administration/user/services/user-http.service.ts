import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { Observable, catchError, tap, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../../../../shared/functions/url-helper';
import { Response } from '../../../../shared/models/respone';
import { GetAllUsersResponse } from '../models/get-all-users-response';
import { InviteUserRequest } from '../models/invite-user-request';
import { UpdateUserRequest } from '../models/update-user-request';
import { UserRole } from '../models/user-role';

@Injectable()
export class UserHttpService extends CacheQuery {
  private baseUrl = getBaseApiUrl() + 'administration/users/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  inviteUser(request: InviteUserRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'inviteuser', request).pipe(
      catchError(() => {
        return throwError(() => new Error(this.translocoService.translate('administration.error.user.invite_user')));
      }),
    );
  }

  getUserRoles(): Observable<Response<UserRole[]>> {
    const key = 'UserHttpService.getUserRoles';
    return this.query(
      key,
      this.httpClient.get<Response<UserRole[]>>(this.baseUrl + 'getuserroles').pipe(
        catchError(() => {
          return throwError(
            () => new Error(this.translocoService.translate('administration.error.user.get_user_roles')),
          );
        }),
        tap((res) => {
          if (!res.isSuccess) {
            this.invalidate(key);
          }
        }),
      ),
    );
  }

  getAllUsers(): Observable<Response<GetAllUsersResponse>> {
    return this.httpClient.get<Response<GetAllUsersResponse>>(this.baseUrl + 'getallusers').pipe(
      catchError(() => {
        return throwError(() => new Error(this.translocoService.translate('administration.error.user.get_all_users')));
      }),
    );
  }

  updateUser(request: UpdateUserRequest): Observable<Response<string>> {
    return this.httpClient.put<Response<string>>(this.baseUrl + 'updateuser', request).pipe(
      catchError(() => {
        return throwError(() => new Error(this.translocoService.translate('administration.error.user.update_user')));
      }),
    );
  }

  deleteUser(id: string): Observable<Response<boolean>> {
    return this.httpClient
      .delete<Response<boolean>>(this.baseUrl + 'deleteuser', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError(() => {
          return throwError(() => new Error(this.translocoService.translate('administration.error.user.delete_user')));
        }),
      );
  }
}
