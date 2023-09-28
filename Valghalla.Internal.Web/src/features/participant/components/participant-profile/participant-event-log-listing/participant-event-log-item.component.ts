import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ParticipantEventLogListingItem } from 'src/features/participant/models/participant-event-log-listing-item';
import { ParticipantHttpService } from 'src/features/participant/services/participant-http.service';
import { FormDialogEvent } from 'src/shared/models/ux/formDialog';
import { SubSink } from 'subsink';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-participant-event-log-item',
  template: `
    <app-form-dialog
      [label]="!data.item ? 'participant.event_log.add_event_log' : 'participant.event_log.edit_event_log'"
      [formGroup]="form"
      (submitEvent)="submit($event)">
      <div class="flex">
        <mat-form-field class="grow" appearance="fill">
          <mat-label>{{ 'shared.common.text' | transloco }}</mat-label>
          <input matInput type="text" formControlName="text" />
        </mat-form-field>
      </div>
    </app-form-dialog>
  `,
  providers: [ParticipantHttpService],
})
export class ParticipantEventLogItemComponent implements OnInit, OnDestroy {
  private subs = new SubSink();

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly participantHttpService: ParticipantHttpService,
    @Inject(MAT_DIALOG_DATA)
    public readonly data: {
      participantId: string;
      item: ParticipantEventLogListingItem;
    },
  ) {}

  readonly form = this.formBuilder.group({
    text: ['', Validators.required],
  });

  ngOnInit(): void {
    if (this.data?.item) {
      this.form.setValue({
        text: this.data.item.text,
      });
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  submit(formEvent: FormDialogEvent) {
    if (!this.data.item) {
      formEvent.pipe(
        this.participantHttpService.createParticipantEventLog({
          participantId: this.data.participantId,
          text: this.form.value.text,
        }),
        (observable, handler) => {
          this.subs.sink = observable.subscribe((res) => {
            if (res.isSuccess) {
              handler.next('success');
            }
          });
        },
      );
    } else {
      formEvent.pipe(
        this.participantHttpService.updateParticipantEventLog({
          id: this.data.item.id,
          text: this.form.value.text,
        }),
        (observable, handler) => {
          this.subs.sink = observable.subscribe((res) => {
            if (res.isSuccess) {
              handler.next('success');
            }
          });
        },
      );
    }
  }
}
