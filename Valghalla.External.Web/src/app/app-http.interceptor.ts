import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpRequest, HttpHandler, HttpResponse } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Response } from 'src/shared/models/respone';
import { NotificationService } from 'src/shared/services/notification.service';
import { TranslocoService } from '@ngneat/transloco';

@Injectable()
export class AppHttpInterceptor implements HttpInterceptor {
  constructor(
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest).pipe(
      tap((res: HttpResponse<Response<any>>) => {
        if (res.ok && res.body?.isSuccess == false && res.body?.errors?.validation) {
          const title = this.translocoService.translate('shared.error.validation');
          const messages = this.wrapMessages(
            res.body.errors.validation.map((msg) => this.translocoService.translate(msg)),
          );
          this.notificationService.showError(messages, title, true);
        } else if (res.ok && res.body?.isSuccess == false) {
          if (res.body.message?.includes('\n')) {
            this.notificationService.showError((res.body.message as any).replaceAll('\n', '</br>'), undefined, true);
          } else {
            this.notificationService.showError(this.translocoService.translate(res.body.message));
          }
        }
      }),
    );
  }

  private wrapMessages(messages: string[]) {
    return (
      '<div class="d-flex flex-column">' +
      messages.map((msg) => '<span>' + this.translocoService.translate(msg) + '</span>').join('') +
      '</div>'
    );
  }
}
