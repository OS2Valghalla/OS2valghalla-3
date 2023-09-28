import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { ElectionType } from '../models/election-type';
import { CreateElectionTypeRequest } from '../models/create-election-type-request';
import { UpdateElectionTypeRequest } from '../models/update-election-type-request';
import { NotificationService } from 'src/shared/services/notification.service';

@Injectable()
export class ElectionTypeHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/electiontype/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getElectionType(id: string): Observable<Response<ElectionType>> {
    return this.httpClient
      .get<Response<ElectionType>>(this.baseUrl + 'getelectiontype', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.election_type.error.get_election_type');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  createElectionType(value: CreateElectionTypeRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createelectiontype', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.election_type.error.create_election_type');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.election_type.success.create_election_type');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateElectionType(value: UpdateElectionTypeRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updateelectiontype', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.election_type.error.update_election_type');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.election_type.success.update_election_type');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  deleteElectionType(id: string): Observable<Response<string>> {
    return this.httpClient
      .delete<Response<string>>(this.baseUrl + 'deleteelectiontype', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.election_type.error.delete_election_type');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.election_type.success.delete_election_type');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }
}
