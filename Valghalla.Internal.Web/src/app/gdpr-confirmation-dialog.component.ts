import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmationDialogData } from 'src/shared/models/ux/confirmation-dialog';

@Component({
  selector: 'app-confirmation-dialog',
  template: `
    <mat-card>
      <mat-card-header mat-dialog-title>
        <mat-card-title>{{ title | transloco }}</mat-card-title>
        <mat-card-subtitle *ngIf="!!subtitle">{{ subtitle | transloco }}</mat-card-subtitle>
      </mat-card-header>
      <mat-card-content mat-dialog-content>
        <div *ngIf="!!content">{{ content | transloco }}</div>
        <div *ngIf="htmlContent" [innerHTML]="htmlContent"></div>
        <mat-form-field>
          <input matInput [(ngModel)]="model" (ngModelChange)="onModelChange()" />
          <mat-hint>{{ 'app.gdpr_confirmation_dialog.help_text' | transloco }}</mat-hint>
        </mat-form-field>
      </mat-card-content>
      <mat-card-actions mat-dialog-actions align="end">
        <button mat-button color="primary" (click)="onSubmit()" [disabled]="disabled">
          <span>{{ 'shared.dialog.ok' | transloco }}</span>
        </button>
        <ng-container *ngIf="data?.additionalButton">
          <button mat-button color="primary" (click)="onAdditionalButtonClick()">
            <span>{{ data.additionalButton.label | transloco }}</span>
          </button>
        </ng-container>
        <button mat-button (click)="onClose()">{{ 'shared.dialog.cancel' | transloco }}</button>
      </mat-card-actions>
    </mat-card>
  `,
})
export class GdprConfirmationDialogComponent {
  constructor(
    private dialogRef: MatDialogRef<GdprConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmationDialogData,
  ) {}

  title: string = this.data?.title || 'shared.dialog.areyousure';

  subtitle: string = this.data?.subtitle;

  content: string = this.data?.content;

  htmlContent: string = this.data?.htmlContent;

  model: string;

  disabled: boolean = true;

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

  onModelChange() {
    if (this.model?.toLowerCase() === 'delete') {
      this.disabled = false;
    } else {
      this.disabled = true;
    }
  }
}
