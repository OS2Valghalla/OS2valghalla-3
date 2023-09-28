import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { catchError, tap, throwError } from 'rxjs';
import { FormDialogEvent } from 'src/shared/models/ux/formDialog';
import { NotificationService } from 'src/shared/services/notification.service';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-form-dialog',
  templateUrl: './form-dialog.component.html',
})
export class FormDialogComponent implements OnInit {
  private subs = new SubSink();

  @Input() label = 'shared.dialog.label';

  @Input() subtitle: string;

  // to make form control work as ng-content, we need workaround solution by using same directive formGroup
  @Input() formGroup: FormGroup;

  @Input() submitButtonLabel: string;

  @Input() hideSubmitButton: boolean;

  @Input() showSubmitButtonOnRight: boolean;

  @Input() pristine: boolean = true;

  @Output() submitEvent = new EventEmitter<FormDialogEvent>();

  submitting = false;

  errors: string[] = [];

  constructor(
    private dialogRef: MatDialogRef<FormDialogComponent>,
    private notificationService: NotificationService,
    private translocoService: TranslocoService,
    private m: BreakpointObserver,
  ) {}

  ngOnInit(): void {
    this.subs.sink = this.m.observe([Breakpoints.Small, Breakpoints.Medium, Breakpoints.Large]).subscribe((state) => {
      if (state.breakpoints[Breakpoints.Small]) {
        this.dialogRef.updateSize('95%');
      } else {
        this.dialogRef.updateSize('900px');
      }
    });
  }

  onSubmit() {
    this.errors = [];

    if (!this.formGroup.valid) {
      return;
    }

    this.submitting = true;
    this.formGroup.disable();

    this.submitEvent.emit({
      pipe: (observable, executor) => {
        observable = observable.pipe(
          catchError((err) => {
            this.submitting = false;
            this.formGroup.enable();

            return throwError(() => err);
          }),
          tap(() => {
            this.formGroup.enable();
            this.submitting = false;
          }),
        );

        executor(observable, {
          next: (data) => {
            if (typeof data === 'string' && data.length > 0) {
              const msgValue = this.translocoService.translate(data);
              this.notificationService.showSuccess(msgValue);
              this.dialogRef.close(data);
            } else if (data) {
              data = data as { msg: string; formResponseData: object };
              const msgValue = this.translocoService.translate(data.msg);
              this.notificationService.showSuccess(msgValue);
              this.dialogRef.close(data.formResponseData);
            } else {
              this.dialogRef.close();
            }
          },
          error: (errors) => {
            this.errors = errors;
            this.submitting = false;
          },
        });
      },
    });
  }

  onClose() {
    this.subs.unsubscribe();
    this.dialogRef.close();
  }
}
