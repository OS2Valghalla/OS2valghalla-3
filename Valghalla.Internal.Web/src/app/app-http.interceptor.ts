import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpRequest, HttpHandler, HttpResponse, HttpClient } from '@angular/common/http';
import { Observable, of, switchMap, take, tap } from 'rxjs';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';
import { GdprConfirmationDialogComponent } from './gdpr-confirmation-dialog.component';
import { ConfirmationDialogComponent } from 'src/shared/components/confirmation-dialog/confirmation-dialog.component';

@Injectable()
export class AppHttpInterceptor implements HttpInterceptor {
  constructor(
    private readonly httpClient: HttpClient,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
    private readonly dialog: MatDialog,
  ) {}

  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest).pipe(
      switchMap((res: HttpResponse<Response<any>>) => {
        if (res.ok && res.body?.confirmation) {
          const confirmation = res.body.confirmation;

          return new Observable<any>((subscriber) => {
            const subscription = httpRequest.url.includes('bulkdeleteparticipants')
              ? this.dialog.open(GdprConfirmationDialogComponent, {
                  data: {
                    title: confirmation.title,
                    htmlContent: this.wrapMessages(
                      confirmation.messages.map((message) => this.translocoService.translate(message)),
                    ),
                  },
                })
              : this.dialog.open(ConfirmationDialogComponent, {
                  data: {
                    title: confirmation.title,
                    htmlContent: this.wrapMessages(
                      confirmation.messages.map((message) => this.translocoService.translate(message)),
                    ),
                  },
                });

            subscription
              .afterClosed()
              .pipe(take(1))
              .subscribe((confirmed) => {
                if (!confirmed) {
                  subscriber.complete();
                  return;
                }

                const newHttpRequest = httpRequest.clone({
                  params: httpRequest.params.append('confirmed', true),
                });

                this.httpClient.request(newHttpRequest).subscribe((res) => {
                  subscriber.next(res);
                });
              });
          });
        }

        return of(res);
      }),
      tap((res: HttpResponse<Response<any>>) => {
        if (res.ok && res.body?.isSuccess == false && res.body?.errors?.validation) {
          const title = this.translocoService.translate('shared.error.validation');
          const messages = this.wrapMessages(
            res.body.errors.validation.map((msg) => this.translocoService.translate(msg)),
          );
          this.notificationService.showError(title, undefined, messages);
        } else if (res.ok && res.body?.isSuccess == false) {
          if (res.body.message?.includes('\n')) {
            this.notificationService.showError(
              undefined,
              undefined,
              (res.body.message as any).replaceAll('\n', '</br>'),
            );
          } else {
            this.notificationService.showError(this.translocoService.translate(res.body.message));
          }
        }
      }),
    );
  }

  private wrapMessages(messages: string[]) {
    return (
      '<div class="flex flex-col space-y-2">' +
      messages.map((msg) => '<span>' + this.translocoService.translate(msg) + '</span>').join('') +
      '</div>'
    );
  }
}
