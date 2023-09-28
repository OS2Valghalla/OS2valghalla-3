import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmationDialogData } from 'src/shared/models/ux/confirmation-dialog';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
})
export class ConfirmationDialogComponent {
  constructor(
    private dialogRef: MatDialogRef<ConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmationDialogData,
  ) {}

  title: string = this.data?.title || 'shared.dialog.areyousure';

  subtitle: string = this.data?.subtitle;

  content: string = this.data?.content;

  htmlContent: string = this.data?.htmlContent;

  onSubmit() {
    this.dialogRef.close(true);
  }

  onClose() {
    this.dialogRef.close(false);
  }

  onAdditionalButtonClick() {
    if (this.data?.additionalButton?.onClick) {
      this.data?.additionalButton?.onClick();
    }

    this.dialogRef.close(true);
  }
}
