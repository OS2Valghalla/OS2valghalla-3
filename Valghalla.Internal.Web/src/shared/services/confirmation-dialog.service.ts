import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../components/confirmation-dialog/confirmation-dialog.component';
import { ConfirmationDialogData } from '../models/ux/confirmation-dialog';

@Injectable({
  providedIn: 'root',
})
export class ConfirmationDialogService {
  constructor(private dialog: MatDialog) {}

  open(
    config: MatDialogConfig<ConfirmationDialogData> = {},
  ): MatDialogRef<ConfirmationDialogComponent, boolean> {
    return this.dialog.open(ConfirmationDialogComponent, {
      disableClose: true,
      ...config,
    });
  }
}
