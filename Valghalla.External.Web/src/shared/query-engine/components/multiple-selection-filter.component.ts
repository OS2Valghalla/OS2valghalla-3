// import { ENTER } from '@angular/cdk/keycodes';
// import {
//   Component,
//   ElementRef,
//   EventEmitter,
//   HostBinding,
//   Input,
//   OnInit,
//   Output,
//   ViewChild,
//   ViewEncapsulation,
// } from '@angular/core';
// import { FormControl } from '@angular/forms';
// import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
// import { map, Observable, startWith } from 'rxjs';
// import { MultipleSelectionFilterValue } from '../models/multiple-selection-filter-value';
// import { SelectOption } from '../models/select-option';

// @Component({
//   selector: 'app-multiple-selection-filter',
//   template: `
//     <mat-form-field class="grow" appearance="fill">
//       <mat-label *ngIf="label">{{ label | transloco }}</mat-label>
//       <mat-chip-grid #chipGrid>
//         <mat-chip-row *ngFor="let option of selectedOptions" (removed)="remove(option)">
//           {{ option.label }}
//           <button matChipRemove>
//             <mat-icon>cancel</mat-icon>
//           </button>
//         </mat-chip-row>
//       </mat-chip-grid>
//       <input
//         #input
//         [formControl]="formControl"
//         [matChipInputFor]="chipGrid"
//         [matAutocomplete]="auto"
//         [matChipInputSeparatorKeyCodes]="separatorKeysCodes" />
//       <mat-autocomplete #auto="matAutocomplete" (optionSelected)="selected($event)">
//         <mat-option *ngFor="let option of filteredOptions | async" [value]="option">
//           {{ option.label }}
//         </mat-option>
//       </mat-autocomplete>
//     </mat-form-field>
//   `,
//   styles: ['app-multiple-selection-filter .mat-expansion-panel-body { display: flex; }'],
//   encapsulation: ViewEncapsulation.None,
// })
// export class MultipleSelectionFilterComponent<T> implements OnInit {
//   @Input() label: string;

//   private _model: MultipleSelectionFilterValue<T>;

//   @Input()
//   set model(value: MultipleSelectionFilterValue<T>) {
//     this._model = value;

//     if (value) {
//       const initialOptions = this._model.values.map((v) => this.options.find((i) => i.value == v));

//       if (initialOptions) {
//         this.selectedOptions = initialOptions;
//       }
//     } else {
//       this.selectedOptions = [];
//     }
//   }
//   get model() {
//     return this._model;
//   }

//   @Output() modelChange = new EventEmitter<MultipleSelectionFilterValue<T>>();

//   @HostBinding('class.flex') flexClass: boolean;

//   @ViewChild('input') input: ElementRef<HTMLInputElement>;

//   options: SelectOption<T>[] = [];

//   selectedOptions: SelectOption<T>[] = [];

//   filteredOptions: Observable<SelectOption<T>[]>;

//   formControl = new FormControl();

//   separatorKeysCodes = [ENTER];

//   ngOnInit(): void {
//     this.flexClass = true;
//     this.filteredOptions = this.formControl.valueChanges.pipe(
//       startWith(null),
//       map((value: string | SelectOption<T> | null) => (typeof value == 'string' ? this._filter(value) : this.options.slice())),
//     );
//   }

//   selected(event: MatAutocompleteSelectedEvent): void {
//     event.option.deselect();
    
//     if (this.selectedOptions.some(i => i.value == event.option.value.value)) {
//       return;
//     }

//     this.input.nativeElement.value = '';
//     this.formControl.setValue(null);
//     this.onSelectionChange([...this.selectedOptions, event.option.value]);
//   }

//   remove(option: SelectOption<T>) {
//     this.onSelectionChange(this.selectedOptions.filter((i) => i.value != option.value));
//   }

//   onSelectionChange(newSelectedOptions: SelectOption<T>[]) {
//     if (!newSelectedOptions || newSelectedOptions.length == 0) {
//       this.model = undefined;
//       this.modelChange.emit(this.model);
//       return;
//     }

//     this.model = {
//       values: newSelectedOptions.map((o) => o.value),
//     };

//     this.modelChange.emit(this.model);
//   }

//   private _filter(value: string): SelectOption<T>[] {
//     return this.options.filter((option) => option.label.toLowerCase().includes(value.toLowerCase()));
//   }
// }
