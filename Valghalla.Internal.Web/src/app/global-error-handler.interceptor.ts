import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable, Injector, isDevMode } from '@angular/core';
import { NotificationService } from '../shared/services/notification.service';
import { ErrorService } from './error.service';
import { isTokenExpiredError } from './auth/utils';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) {}

  handleError(error: any) {
    const errorService = this.injector.get(ErrorService);
    const notifier = this.injector.get(NotificationService);

    let message;

    if (error instanceof HttpErrorResponse) {
      // Server Error
      message = errorService.getServerMessage(error);
    } else {
      // Client Error
      message = errorService.getClientMessage(error);
    }

    if (!isTokenExpiredError(error)) {
      notifier.showError(message); 
    }

    if (isDevMode()) console.error(error);
  }
}
