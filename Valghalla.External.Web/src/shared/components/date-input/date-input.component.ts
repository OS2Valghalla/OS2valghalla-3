import { Component, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { FormControlDirective } from '../../directives/form/standalone/form-control.directive';

@Component({
  selector: 'app-date-input',
  templateUrl: './date-input.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: DateInputComponent,
      multi: true,
    },
  ],
  hostDirectives: [FormControlDirective],
})
export class DateInputComponent implements ControlValueAccessor {
  readonly dayInputId = crypto.randomUUID();
  readonly monthInputId = crypto.randomUUID();
  readonly yearInputId = crypto.randomUUID();

  @Input() required: boolean;

  day: number;
  month: number;
  year: number;

  dayChanged: boolean = false;
  monthChanged: boolean = false;
  yearChanged: boolean = false;

  onChange: (value: Date) => void;
  onTouch: (value: Date) => void;

  onValueChange() {
    if ((!this.year || !this.month || !this.day) && this.dayChanged && this.monthChanged && this.yearChanged) {
      this.onChange(null);
      this.onTouch(null);
      return;
    }

    const value = new Date(this.year, this.month - 1, this.day);

    this.onChange(value);
    this.onTouch(value);
  }

  writeValue(value: Date | string): void {
    if (value != null) {
      const dateValue = typeof value === 'string' ? new Date(value) : value;

      this.day = dateValue.getDay();
      this.month = dateValue.getMonth() + 1;
      this.year = dateValue.getFullYear();
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouch = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    //throw new Error('Method not implemented.');
  }
}
