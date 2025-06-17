import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { CreateElectionRequest } from '../models/create-election-request';
import { UpdateElectionRequest } from '../models/update-election-request';
import { DuplicateElectionRequest } from '../models/duplicate-election-request';
import { UpdateElectionCommunicationConfigurationsRequest } from '../models/update-election-communication-configurations-request';
import { NotificationService } from 'src/shared/services/notification.service';
import { ElectionDetails } from '../models/election-details';
import { ElectionCommunicationConfigurations } from '../models/election-communication-configurations';

@Injectable()
export class ElectionHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/election/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getElection(id: string): Observable<Response<ElectionDetails>> {
    return this.httpClient
      .get<Response<ElectionDetails>>(this.baseUrl + 'getelection', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.election.error.get_election');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
  getAllElections(): Observable<Response<ElectionDetails[]>> {
    return this.httpClient
      .get<Response<ElectionDetails[]>>(this.baseUrl + 'getelections', {        
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.election.error.get_election');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }
  getElectionCommunicationConfigurations(id: string): Observable<Response<ElectionCommunicationConfigurations>> {
    return this.httpClient
      .get<Response<ElectionCommunicationConfigurations>>(this.baseUrl + 'getelectioncommunicationconfigurations', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate(
            'administration.election.error.get_election_communication_configurations',
          );
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
      );
  }

  createElection(value: CreateElectionRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createelection', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.election.error.create_election');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.election.success.create_election');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateElection(value: UpdateElectionRequest): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updateelection', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.election.error.update_election');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.election.success.update_election');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  duplicateElection(value: DuplicateElectionRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'duplicateelection', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.election.error.duplicate_election');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate('administration.election.success.duplicate_election');
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  updateElectionCommunicationConfigurations(
    value: UpdateElectionCommunicationConfigurationsRequest,
  ): Observable<Response<void>> {
    return this.httpClient.put<Response<void>>(this.baseUrl + 'updateelectioncommunicationconfigurations', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate(
          'administration.election.error.update_election_communication_configurations',
        );
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
        if (res.isSuccess) {
          const msg = this.translocoService.translate(
            'administration.election.success.update_election_communication_configurations',
          );
          this.notificationService.showSuccess(msg);
        }
      }),
    );
  }

  activateElection(id: string): Observable<Response<void>> {
    return this.httpClient
      .post<Response<void>>(this.baseUrl + 'activateelection', null, {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.election.error.activate_election');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.election.success.activate_election');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  deactivateElection(id: string): Observable<Response<void>> {
    return this.httpClient
      .post<Response<void>>(this.baseUrl + 'deactivateelection', null, {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.election.error.deactivate_election');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.election.success.deactivate_election');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  deleteElection(id: string): Observable<Response<void>> {
    return this.httpClient
      .delete<Response<void>>(this.baseUrl + 'deleteelection', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError((err) => {
          const msg = this.translocoService.translate('administration.election.error.delete_election');
          this.notificationService.showError(msg);

          return throwError(() => err);
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.election.success.delete_election');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }
}
