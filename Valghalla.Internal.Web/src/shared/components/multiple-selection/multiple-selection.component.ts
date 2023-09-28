import { ENTER } from '@angular/cdk/keycodes';
import {
  Component,
  ElementRef,
  EventEmitter,
  Inject,
  Injector,
  Input,
  Output,
  ViewChild,
  forwardRef,
} from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NG_VALUE_ACCESSOR,
  FormControlDirective,
  NgControl,
  ControlContainer,
  Validators,
} from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Observable, map, startWith } from 'rxjs';

@Component({
  selector: 'app-multiple-selection',
  templateUrl: './multiple-selection.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MultipleSelectionComponent),
      multi: true,
    },
  ],
})
export class MultipleSelectionComponent<T extends { alternative?: boolean; [k: string]: any }>
  implements ControlValueAccessor
{
  @Input() label: string;

  @Input() items: Array<T>;

  @Input() valueProperty: keyof T;

  @Input() displayProperty: keyof T;

  @Input() alternative: boolean;

  @Output() alternativeEvent = new EventEmitter<void>();

  @ViewChild('input') input: ElementRef<HTMLInputElement>;

  @Input() formControlName: string;

  selectedItems: Array<T> = [];

  filteredItems: Observable<Array<T>>;

  formControl = new FormControl();

  separatorKeysCodes = [ENTER];

  required: boolean;

  private onChange: (value: Array<unknown>) => void;
  private onTouch: (value: Array<unknown>) => void;

  constructor(private readonly controlContainer: ControlContainer) {}

  ngOnInit(): void {
    if (this.formControlName) {
      const providedFormControl = this.controlContainer.control.get(this.formControlName);
      this.required = providedFormControl.hasValidator(Validators.required);
    }

    this.filteredItems = this.formControl.valueChanges.pipe(
      startWith(null),
      map((value: string | Array<T> | null) =>
        this.alternative
          ? this.items.filter((i) => i.alternative)
          : typeof value == 'string'
          ? this._filter(value)
          : this.items.slice(),
      ),
    );

    if (this.alternative) {
      this.formControl.disable();
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    if (this.alternative) {
      this.alternativeEvent.emit();
      event.option.deselect();
      return;
    }

    event.option.deselect();

    if (this.selectedItems.some((item) => item[this.valueProperty] == event.option.value[this.valueProperty])) {
      return;
    }

    this.input.nativeElement.value = '';
    this.formControl.setValue(null);
    this.onSelectionChange([...this.selectedItems, event.option.value]);
  }

  remove(itemToRemove: T) {
    this.onSelectionChange(
      this.selectedItems.filter((item) => item[this.valueProperty] != itemToRemove[this.valueProperty]),
    );
  }

  onSelectionChange(newSelectedItems: Array<T>) {
    if (!newSelectedItems || newSelectedItems.length == 0) {
      this.selectedItems = [];
      this.onChange([]);
      this.onTouch([]);
      return;
    }

    this.selectedItems = newSelectedItems;
    const values = newSelectedItems.map((item) => item[this.valueProperty]);

    this.onChange(values);
    this.onTouch(values);
  }

  writeValue(values: Array<unknown>): void {
    this.selectedItems = !values ? [] : this.items.filter((item) => values.includes(item[this.valueProperty]));
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouch = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    if (isDisabled) {
      this.formControl.disable();
    } else if (!this.alternative) {
      this.formControl.enable();
    }
  }

  private _filter(value: string): Array<T> {
    return this.items.filter((item) =>
      (item[this.displayProperty] as string).toLowerCase().includes(value.toLowerCase()),
    );
  }
}
