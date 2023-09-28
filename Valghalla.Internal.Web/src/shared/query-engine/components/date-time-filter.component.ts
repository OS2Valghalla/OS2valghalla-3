import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DateTimeFilterValue } from '../models/date-time-filter-value';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';

@Component({
  selector: 'app-date-time-filter',
  template: `
    <mat-form-field class="grow w-full" appearance="fill">
      <mat-label>{{ label | transloco }}</mat-label>
      <input matInput [matDatepicker]="picker" [value]="model?.value" (dateChange)="onChange($event)" />
      <mat-datepicker-toggle matIconSuffix [for]="picker"></mat-datepicker-toggle>
      <mat-datepicker #picker></mat-datepicker>
    </mat-form-field>
  `,
})
export class DateTimeFilterComponent {
  @Input() label: string;

  @Input() model: DateTimeFilterValue;

  @Output() modelChange = new EventEmitter<DateTimeFilterValue>();

  onChange(event: MatDatepickerInputEvent<any, any>) {
    this.model = {
      value: event.target.value,
    };

    this.modelChange.emit(this.model);
  }
}
