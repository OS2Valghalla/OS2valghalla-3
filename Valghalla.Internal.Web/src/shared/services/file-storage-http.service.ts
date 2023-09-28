import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError } from 'rxjs';
import { CacheQuery } from 'src/shared/state/cache-query';
import { getBaseApiUrl } from '../functions/url-helper';
import { Response } from '../models/respone';

@Injectable({
  providedIn: 'root',
})
export class FileStorageHttpService extends CacheQuery {
  private baseUrl = getBaseApiUrl() + 'shared/filestorage/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {
    super();
  }

  getDownloadFileLink(id: string) {
    return this.baseUrl + 'download?id=' + id;
  }

  uploadFile(file: File, fileName: string, type: string): Observable<Response<string>> {
    const formData = new FormData();
    formData.append('content', file, fileName);

    return this.httpClient
      .post<Response<string>>(this.baseUrl + 'upload', formData, {
        params: {
          type: type,
        },
      })
      .pipe(
        catchError(() => {
          return throwError(() => new Error(this.translocoService.translate('shared.error.upload_file')));
        }),
      );
  }

  deleteFile(id: string): Observable<Response<void>> {
    return this.httpClient
      .delete<Response<void>>(this.baseUrl + 'delete', {
        params: {
          id: id,
        },
      })
      .pipe(
        catchError(() => {
          return throwError(() => new Error(this.translocoService.translate('shared.error.delete_file')));
        }),
      );
  }
}
