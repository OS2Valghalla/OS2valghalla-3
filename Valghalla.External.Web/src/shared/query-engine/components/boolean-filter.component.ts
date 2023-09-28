// import { Component, EventEmitter, HostBinding, Input, OnInit, Output } from '@angular/core';
// import { BooleanFilterValue } from '../models/boolean-filter-value';

// type RadioValue = 'yes' | 'no' | 'all';

// @Component({
//   selector: 'app-boolean-filter',
//   template: `
//     <div class="form-group">
//       <fieldset role="radiogroup">
//         <legend class="form-label">{{ label | transloco }}</legend>

//         <div class="form-group-radio">
//           <input type="radio" [attr.id]="id + '-yes'" [attr.name]="id + '-yes'" class="form-radio" value="yes" />
//           <label class="form-label" [attr.for]="id + '-yes'">{{ 'shared.query_engine.boolean.yes' | transloco }}</label>
//         </div>

//         <div class="form-group-radio">
//           <input type="radio" [attr.id]="id + '-no'" [attr.name]="id + '-no'" class="form-radio" value="no" />
//           <label class="form-label" [attr.for]="id + '-no'">{{ 'shared.query_engine.boolean.no' | transloco }}</label>
//         </div>

//         <div class="form-group-radio">
//           <input type="radio" [attr.id]="id + '-all'" [attr.name]="id + '-all'" class="form-radio" value="all" />
//           <label class="form-label" [attr.for]="id + '-all'">{{ 'shared.query_engine.boolean.all' | transloco }}</label>
//         </div>
//       </fieldset>
//     </div>
//   `,
// })
// export class BooleanFilterComponent implements OnInit {
//   readonly id = crypto.randomUUID();

//   @Input() label: string;

//   private _model: BooleanFilterValue;

//   @Input()
//   set model(value: BooleanFilterValue) {
//     this._model = value;

//     if (!this.model) {
//       this.radioValue = 'all';
//     } else if (this.model.value) {
//       this.radioValue = 'yes';
//     } else {
//       this.radioValue = 'no';
//     }
//   }
//   get model() {
//     return this._model;
//   }

//   @Output() modelChange = new EventEmitter<BooleanFilterValue>();

//   @HostBinding('class.flex') flexClass: boolean;

//   radioValue: RadioValue;

//   ngOnInit(): void {
//     this.flexClass = true;
//   }

//   onChange(event: any) {
//     if (event.target.value == 'all') {
//       this._model = undefined;
//     } else if (event.target.value == 'yes') {
//       this._model = { value: true };
//     } else if (event.target.value == 'no') {
//       this._model = { value: false };
//     }

//     this.modelChange.emit(this._model);
//   }
// }
