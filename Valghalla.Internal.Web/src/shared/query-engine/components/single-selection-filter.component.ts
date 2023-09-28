import {
  Component,
  ElementRef,
  EventEmitter,
  HostBinding,
  Input,
  OnInit,
  Output,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { SelectOption } from '../models/select-option';
import { SingleSelectionFilterValue } from '../models/single-selection-filter-value';
import { ENTER } from '@angular/cdk/keycodes';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { map, Observable, startWith } from 'rxjs';

@Component({
  selector: 'app-single-selection-filter',
  template: `
    <mat-form-field class="grow" appearance="fill">
      <mat-label *ngIf="label">{{ label | transloco }}</mat-label>
      <mat-chip-grid #chipGrid>
        <mat-chip-row *ngFor="let option of selectedOptions" (removed)="remove()">
          <ng-container *ngIf="!localized">{{ option.label }}</ng-container>
          <ng-container *ngIf="localized">{{ option.label | transloco }}</ng-container>
          <button matChipRemove>
            <mat-icon>cancel</mat-icon>
          </button>
        </mat-chip-row>
      </mat-chip-grid>
      <input
        #input
        [formControl]="formControl"
        [matChipInputFor]="chipGrid"
        [matAutocomplete]="auto"
        [matChipInputSeparatorKeyCodes]="separatorKeysCodes" />
      <mat-autocomplete #auto="matAutocomplete" (optionSelected)="selected($event)">
        <mat-option *ngFor="let option of filteredOptions | async" [value]="option">
          <ng-container *ngIf="!localized">{{ option.label }}</ng-container>
          <ng-container *ngIf="localized">{{ option.label | transloco }}</ng-container>
        </mat-option>
      </mat-autocomplete>
    </mat-form-field>
  `,
  styles: ['app-single-selection-filter .mat-expansion-panel-body { display: flex; }'],
  encapsulation: ViewEncapsulation.None,
})
export class SingleSelectionFilterComponent<T> implements OnInit {
  @Input() label: string;

  private _model: SingleSelectionFilterValue<T>;
  private _options: SelectOption<T>[];

  @Input()
  set model(value: SingleSelectionFilterValue<T>) {
    this._model = value;

    if (value) {
      const initialOption = this.options.find((i) => i.value == this._model?.value);

      if (initialOption) {
        this.selectedOptions = [initialOption];
        this.formControl.disable();
      }
    } else {
      this.selectedOptions = [];
      this.formControl.enable();
    }
  }
  get model() {
    return this._model;
  }

  @Input()
  set options(value: SelectOption<T>[]) {
    this._options = value;
    this.formControl.setValue(null); 
  }
  get options() {
    return this._options;
  }

  @Input() localized: boolean;

  @Output() modelChange = new EventEmitter<SingleSelectionFilterValue<T>>();

  @HostBinding('class.flex') flexClass: boolean;

  @ViewChild('input') input: ElementRef<HTMLInputElement>;

  selectedOptions: SelectOption<T>[] = [];

  filteredOptions: Observable<SelectOption<T>[]>;

  formControl = new FormControl();

  separatorKeysCodes = [ENTER];

  ngOnInit(): void {
    this.flexClass = true;
    this.filteredOptions = this.formControl.valueChanges.pipe(
      startWith(null),
      map((text: string | SelectOption<T> | null) => (text ? this._filter(text) : this.options.slice())),
    );
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    event.option.deselect();
    
    this.input.nativeElement.value = '';
    this.formControl.setValue(null);
    this.onSelectionChange(event.option.value.value);
  }

  remove() {
    this.onSelectionChange(undefined);
  }

  onSelectionChange(value: T) {
    if (typeof value === 'undefined' || value == null) {
      this.model = undefined;
      this.modelChange.emit(this.model);
      return;
    }

    this.model = {
      value: value,
    };

    this.modelChange.emit(this.model);
  }

  private _filter(value: string | SelectOption<T>): SelectOption<T>[] {
    const filterValue = typeof value === 'string' ? value.toLowerCase() : value.label.toLowerCase();
    return this.options.filter((option) => option.label.toLowerCase().includes(filterValue));
  }
}
