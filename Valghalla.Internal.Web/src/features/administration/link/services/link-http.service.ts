import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { Response } from 'src/shared/models/respone';
import { CreateFilteredTaskLinkRequest } from '../models/create-filtered-tasks-link-request';
import { NotificationService } from 'src/shared/services/notification.service';

@Injectable()
export class LinkHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/link/';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  createFilteredTasksLink(value: CreateFilteredTaskLinkRequest): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(this.baseUrl + 'createtasksfilteredlink', value).pipe(
      catchError((err) => {
        const msg = this.translocoService.translate('administration.error.link.create_filtered_tasks_link_failed');
        this.notificationService.showError(msg);

        return throwError(() => err);
      }),
      tap((res) => {
      }),
    );
  }
}