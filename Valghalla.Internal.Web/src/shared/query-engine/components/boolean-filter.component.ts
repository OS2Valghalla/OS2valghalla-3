import {
  Component,
  EventEmitter,
  HostBinding,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { MatRadioChange } from '@angular/material/radio';
import { BooleanFilterValue } from '../models/boolean-filter-value';

type RadioValue = 'yes' | 'no' | 'all';

@Component({
  selector: 'app-boolean-filter',
  template: `
    <mat-label *ngIf="label">{{ label | transloco }}</mat-label>
    <mat-radio-group
      class="flex"
      [value]="radioValue"
      (change)="onChange($event)">
      <mat-radio-button value="yes">{{
        'shared.query_engine.boolean.yes' | transloco
      }}</mat-radio-button>
      <mat-radio-button value="no">{{
        'shared.query_engine.boolean.no' | transloco
      }}</mat-radio-button>
      <mat-radio-button value="all">{{
        'shared.query_engine.boolean.all' | transloco
      }}</mat-radio-button>
    </mat-radio-group>
  `,
})
export class BooleanFilterComponent implements OnInit {
  @Input() label: string;

  private _model: BooleanFilterValue;

  @Input()
  set model(value: BooleanFilterValue) {
    this._model = value;

    if (!this.model) {
      this.radioValue = 'all';
    } else if (this.model.value) {
      this.radioValue = 'yes';
    } else {
      this.radioValue = 'no';
    }
  }
  get model() {
    return this._model;
  }

  @Output() modelChange = new EventEmitter<BooleanFilterValue>();

  @HostBinding('class.flex') flexClass: boolean;

  radioValue: RadioValue;

  ngOnInit(): void {
    this.flexClass = true;
  }

  onChange(event: MatRadioChange) {
    if (event.value == 'all') {
      this._model = undefined;
    } else if (event.value == 'yes') {
      this._model = { value: true };
    } else if (event.value == 'no') {
      this._model = { value: false };
    }

    this.modelChange.emit(this._model);
  }
}
