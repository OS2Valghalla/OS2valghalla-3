// import {
//   AfterViewInit,
//   Component,
//   ComponentRef,
//   ContentChild,
//   EventEmitter,
//   Input,
//   OnDestroy,
//   Output,
//   ViewChild,
//   ViewContainerRef,
// } from '@angular/core';
// import { QueryChangeEvent, QueryForm } from '../models/query-form';
// import { QueryFormPropertyType } from '../models/query-form-info';
// import { BooleanFilterComponent } from './boolean-filter.component';
// import { FreeTextSearchComponent } from './free-text-search.component';
// import { MultipleSelectionFilterComponent } from './multiple-selection-filter.component';
// import { SingleSelectionFilterComponent } from './single-selection-filter.component';
// import { QueryContainerComponent } from './query-container.component';
// import { SubSink } from 'subsink';
// import { DynamicQueryContentDirective } from '../directives/dynamic-query-content.directive';
// import { TranslocoService } from '@ngneat/transloco';
// import { SelectOption } from '../models/select-option';

// @Component({
//   selector: 'app-dynamic-query',
//   template: `
//     <ng-template #ref></ng-template>
//     <ng-container
//       *ngIf="contentDirective"
//       [ngTemplateOutlet]="contentDirective.templateRef"
//       [ngTemplateOutletContext]="{ $implicit: this.model }"></ng-container>
//   `,
// })
// export class DynamicQueryComponent<TQueryForm extends QueryForm, PropertyName extends keyof TQueryForm>
//   implements AfterViewInit, OnDestroy
// {
//   private subs = new SubSink();

//   @Input() typing: TQueryForm; // typing

//   @Input() label: string;

//   @Input() localized: boolean;

//   @Input() name: PropertyName;

//   @Output() queryChangeEvent = new EventEmitter<QueryChangeEvent<TQueryForm, any>>();

//   @ViewChild('ref', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;

//   @ContentChild(DynamicQueryContentDirective) contentDirective: DynamicQueryContentDirective<any>;

//   model: unknown;

//   componentRef: ComponentRef<any>;

//   constructor(
//     private readonly container: QueryContainerComponent<TQueryForm, unknown, unknown>,
//     private readonly translocoService: TranslocoService,
//   ) {}

//   ngAfterViewInit(): void {
//     this.subs.sink = this.container.queryForm$.subscribe((form) => {
//       if (this.componentRef && form[this.name] !== this.model) {
//         this.model = form[this.name];
//         this.componentRef.instance.model = this.model;

//         if (typeof this.model == 'undefined' || this.model == null) {
//           this.container.clearAppliedFilter(this.name as string);
//         }
//       } 
//     });

//     this.subs.sink = this.container.filters$.subscribe((filters) => {
//       const filteredOptions: SelectOption<any>[] = filters[this.name as string];

//       if (this.componentRef && this.componentRef.instance.options && filteredOptions && filteredOptions.length != this.componentRef.instance.options.length) {
//         this.componentRef.instance.options = filteredOptions;
//       }
//     });

//     this.subs.sink = this.container.properties$.subscribe((properties) => {
//       this.viewContainerRef.clear();

//       const propertyType = properties[this.name as string];

//       if (propertyType == QueryFormPropertyType.Boolean) {
//         const componentRef = this.viewContainerRef.createComponent(BooleanFilterComponent);
//         componentRef.instance.label = this.label;
//         componentRef.instance.model = this.model = this.container.queryForm.value[this.name as string];

//         this.subs.sink = componentRef.instance.modelChange.subscribe((model) => {
//           this.model = model;
//           const text = !model ? 'all' : model.value ? 'yes' : 'no';
//           this.container.updateQuery(this.name as string, model, this.label, text);
//           this.hook(model);
//         });

//         this.componentRef = componentRef;
//       } else if (propertyType == QueryFormPropertyType.FreeText) {
//         const componentRef = this.viewContainerRef.createComponent(FreeTextSearchComponent);
//         componentRef.instance.label = this.label;
//         componentRef.instance.model = this.model = this.container.queryForm.value[this.name as string];

//         this.subs.sink = componentRef.instance.modelChange.subscribe((model) => {
//           this.model = model;
//           this.container.updateQuery(this.name as string, model, this.label, model?.value);
//           this.hook(model);
//         });

//         this.componentRef = componentRef;
//       } else if (propertyType == QueryFormPropertyType.SingleSelection) {
//         const componentRef = this.viewContainerRef.createComponent(SingleSelectionFilterComponent);
//         componentRef.instance.label = this.label;
//         componentRef.instance.options = this.container.filters.value[this.name as string];
//         componentRef.instance.model = this.model = this.container.queryForm.value[this.name as string];
//         componentRef.instance.localized = this.localized;

//         this.subs.sink = componentRef.instance.modelChange.subscribe((model) => {
//           this.model = model;
//           const text = componentRef.instance.options.find((i) => i.value == model?.value)?.label;
//           const localizedText = this.localized ? this.translocoService.translate(text) : text;
//           this.container.updateQuery(this.name as string, model, this.label, localizedText);
//           this.hook(model);
//         });

//         this.componentRef = componentRef;
//       } else if (propertyType == QueryFormPropertyType.MutipleSelection) {
//         const componentRef = this.viewContainerRef.createComponent(MultipleSelectionFilterComponent);
//         componentRef.instance.label = this.label;
//         componentRef.instance.options = this.container.filters.value[this.name as string];
//         componentRef.instance.model = this.model = this.container.queryForm.value[this.name as string];

//         this.subs.sink = componentRef.instance.modelChange.subscribe((model) => {
//           this.model = model;
//           const text = model?.values.map((v) => componentRef.instance.options.find((i) => i.value == v)?.label);
//           this.container.updateQuery(this.name as string, model, this.label, text);
//           this.hook(model);
//         });

//         this.componentRef = componentRef;
//       }
//     });
//   }

//   private hook(model: any) {
//     this.queryChangeEvent.emit({
//       form: this.container.queryForm,
//       filters: this.container.filters,
//       originalFilters: this.container.originalFilters,
//       model: model,
//     });
//   }

//   ngOnDestroy(): void {
//     this.subs.unsubscribe();
//   }
// }
