import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, Observable, throwError } from 'rxjs';
import { getBaseApiUrl } from '../functions/url-helper';
import { TemplateFile } from '../models/printing/template-file';
import { Response } from '../models/respone';

@Injectable()
export class PrintingHttpService {
  private baseUrl = getBaseApiUrl() + 'print/';

  constructor(private httpClient: HttpClient, private translocoService: TranslocoService) {}

  getDocxTemplates(electionId: string, templateTypeName: string): Observable<Response<TemplateFile[]>> {
    return this.httpClient
      .get<Response<TemplateFile[]>>(this.baseUrl + 'getdocxtemplates', {
        params: { electionId: electionId, templateTypeName: templateTypeName },
      })
      .pipe(
        catchError(() => {
          return throwError(() => new Error(this.translocoService.translate('shared.error.error_getting_templates')));
        }),
      );
  }
}
