import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { NotificationService } from 'src/shared/services/notification.service';
import { Response } from 'src/shared/models/respone';
import { ParticipantRegisterRequest } from '../models/participant-register-request';
import { MyProfileRegistration } from '../models/my-profile-registration';
import { TeamAccessStatus } from '../models/team-access-status';

@Injectable()
export class RegistrationHttpService {
  private baseUrl = getBaseApiUrl() + 'registration/';

  constructor(
    private httpClient: HttpClient,
    private translocoService: TranslocoService,
    private notificationService: NotificationService,
  ) {}

  registerParticipantWithTeam(value: ParticipantRegisterRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'registerwithteam', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('registration.error.register_participant');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  registerParticipantWithTask(
    value: ParticipantRegisterRequest,
    invitationCode?: string,
  ): Observable<Response<string>> {
    let query = {};

    if (invitationCode) {
      query = {
        ...query,
        ...{
          code: invitationCode,
        },
      };
    }

    return this.httpClient
      .post<Response<string>>(this.baseUrl + 'registerwithtask', value, {
        params: query,
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('registration.error.register_participant');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  joinTeam(hashValue: string) {
    return this.httpClient
      .post<Response<void>>(this.baseUrl + 'jointeam', undefined, {
        params: {
          hashValue: hashValue,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('registration.error.join_team');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('registration.success.join_team');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  getMyProfileRegistration() {
    return this.httpClient.get<Response<MyProfileRegistration>>(this.baseUrl + 'getmyprofileregistration').pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('registration.error.get_my_profile_registration');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
    );
  }

  getTeamAccessStatus(hashValue: string) {
    return this.httpClient
      .get<Response<TeamAccessStatus>>(this.baseUrl + 'getteamaccessstatus', {
        params: {
          hashValue: hashValue,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('registration.error.get_team_access_status');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
}
