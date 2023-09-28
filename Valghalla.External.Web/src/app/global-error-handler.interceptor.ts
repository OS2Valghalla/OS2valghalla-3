import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable, Injector, isDevMode } from '@angular/core';
import { ErrorService } from './error.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) {}

  handleError(error: any) {
    const errorService = this.injector.get(ErrorService);

    let message;

    if (error instanceof HttpErrorResponse) {
      // Server Error
      message = errorService.getServerMessage(error);
    } else {
      // Client Error
      message = errorService.getClientMessage(error);
    }

    if (isDevMode()) console.error(error);
  }
}
