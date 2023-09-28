import { Injectable, NgZone } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AppSnackbarComponent } from '../components/snackbar/snackbar.component';
import { SnackTypes } from '../models/snackbar-data';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  constructor(
    public snackBar: MatSnackBar,
    private zone: NgZone,
  ) {}

  showSuccess(message: string, body?: string): void {
    this.zone.run(() => {
      this.snackBar.openFromComponent(AppSnackbarComponent, {
        duration: 3000,
        verticalPosition: 'top',
        horizontalPosition: 'right',
        panelClass: 'success-snackbar',
        data: { 
          message: message, 
          body: body, 
          snackType: SnackTypes.Success },
      });
    });
  }

  showError(message: string, body?: string, htmlContent?: string): void {
    this.zone.run(() => {
      this.snackBar.openFromComponent(AppSnackbarComponent, {
        verticalPosition: 'top',
        horizontalPosition: 'right',
        panelClass: 'error-snackbar',
        data: {
          message: message,
          body: body,
          htmlContent: htmlContent,
          snackType: SnackTypes.Error,
          showCloseButton: true,
        },
      });
    });
  }

  showWarning(message: string, body?: string): void {
    this.zone.run(() => {
      this.snackBar.openFromComponent(AppSnackbarComponent, {
        duration: 3000,
        verticalPosition: 'top',
        horizontalPosition: 'right',
        panelClass: 'warning-snackbar',
        data: { 
          message: message, 
          body: body, 
          snackType: SnackTypes.Warning },
      });
    });
  }

  showInfo(message: string, body?: string): void {
    this.zone.run(() => {
      this.snackBar.openFromComponent(AppSnackbarComponent, {
        duration: 3000,
        verticalPosition: 'top',
        horizontalPosition: 'right',
        panelClass: 'info-snackbar',
        data: { 
          message: message, 
          body: body, 
          snackType: SnackTypes.Info },
      });
    });
  }
}
