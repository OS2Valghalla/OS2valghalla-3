import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError, tap } from 'rxjs';
import { TranslocoService } from '@ngneat/transloco';
import { Response } from '../../../../shared/models/respone';
import { WebPage } from '../models/web-page';
import { DeclarationOfConsentPage } from '../models/declaration-of-consent-page';
import { FrontPage } from '../models/front-page';
import { FAQPage } from '../models/faq-page';
import { ContactInformation } from '../models/contact-information';
import { UpdateWebPageRequest } from '../models/update-web-page-request';
import { UpdateDeclarationOfConsentPageRequest } from '../models/update-declaration-of-consent-page-request';
import { UpdateFrontPageRequest } from '../models/update-front-page-request';
import { UpdateFAQPageRequest } from '../models/update-faq-page-request';
import { UpdateContactInformationRequest } from '../models/update-contact-information-request';
import { getBaseApiUrl } from '../../../../shared/functions/url-helper';
import { NotificationService } from 'src/shared/services/notification.service';

@Injectable()
export class WebPageHttpService {
  private baseUrl = getBaseApiUrl() + 'administration/web/';

  constructor(
    private httpClient: HttpClient,
    private translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  getFrontPage(): Observable<Response<FrontPage>> {
    return this.httpClient
      .get<Response<FrontPage>>(this.baseUrl + 'front')
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'administration.web.error.get_front',
                ),
              ),
          );
        }),        
      );
  }

  updateFrontPage(value: UpdateFrontPageRequest): Observable<Response<void>> {
    return this.httpClient
      .put<Response<void>>(this.baseUrl + 'front', value)
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'administration.web.error.update_front',
                ),
              ),
          );
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.web.success.update_front');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  getFAQPage(): Observable<Response<FAQPage>> {
    return this.httpClient
      .get<Response<FAQPage>>(this.baseUrl + 'faq')
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'administration.web.error.get_faq',
                ),
              ),
          );
        }),        
      );
  }

  updateFAQPage(value: UpdateFAQPageRequest): Observable<Response<void>> {
    return this.httpClient
      .put<Response<void>>(this.baseUrl + 'faq', value)
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'administration.web.error.update_faq',
                ),
              ),
          );
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.web.success.update_faq');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  getDisclosureStatementPage(): Observable<Response<WebPage>> {
    return this.httpClient
      .get<Response<WebPage>>(this.baseUrl + 'disclosurestatement')
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'administration.web.error.get_disclosure_statement',
                ),
              ),
          );
        }),        
      );
  }

  updateDisclosureStatementPage(value: UpdateWebPageRequest): Observable<Response<void>> {
    return this.httpClient
      .put<Response<void>>(this.baseUrl + 'disclosurestatement', value)
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'administration.web.error.update_disclosure_statement',
                ),
              ),
          );
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.web.success.update_disclosure_statement');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  getDeclarationOfConsentPage(): Observable<Response<DeclarationOfConsentPage>> {
    return this.httpClient
      .get<Response<DeclarationOfConsentPage>>(this.baseUrl + 'declarationofconsent')
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'administration.web.error.get_declaration_of_consent',
                ),
              ),
          );
        }),        
      );
  }

  updateDeclarationOfConsentPage(value: UpdateDeclarationOfConsentPageRequest): Observable<Response<void>> {
    return this.httpClient
      .put<Response<void>>(this.baseUrl + 'declarationofconsent', value)
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'administration.web.error.update_declaration_of_consent',
                ),
              ),
          );
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.web.success.update_declaration_of_consent');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }

  getContactInformation(): Observable<Response<ContactInformation>> {
    return this.httpClient
      .get<Response<ContactInformation>>(this.baseUrl + 'contactinformation')
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'administration.web.error.get_contact_information',
                ),
              ),
          );
        }),        
      );
  }

  updateContactInformation(value: UpdateContactInformationRequest): Observable<Response<void>> {
    return this.httpClient
      .put<Response<void>>(this.baseUrl + 'contactinformation', value)
      .pipe(
        catchError(() => {
          return throwError(
            () =>
              new Error(
                this.translocoService.translate(
                  'administration.web.error.update_contact_information',
                ),
              ),
          );
        }),
        tap((res) => {
          if (res.isSuccess) {
            const msg = this.translocoService.translate('administration.web.success.update_contact_information');
            this.notificationService.showSuccess(msg);
          }
        }),
      );
  }
}
