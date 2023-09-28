// import { AfterViewInit, Component, ContentChild, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
// import { BehaviorSubject, ReplaySubject, Subscription } from 'rxjs';
// import { TableComponent } from 'src/shared/components/table/table.component';
// import { TablePageEvent } from 'src/shared/models/ux/table';
// import { SubSink } from 'subsink';
// import { QueryContainerContentDirective } from '../directives/query-container-content.directive';
// import { FreeTextSearchValue } from '../models/free-text-search-value';
// import { Order } from '../models/order';
// import {
//   QueryClientSourceQueryEvent,
//   QueryEvent,
//   QueryForm,
//   QueryFormEvent,
//   QueryPrepareEvent,
// } from '../models/query-form';
// import { QueryFilters, QueryFormPropertyType } from '../models/query-form-info';
// import { QueryResult } from '../models/query-result';
// import { QueryEngineClient } from '../query-engine-client';

// interface AppliedFilter {
//   name: string;
//   label: string;
//   value: any;
//   text: string | string[];
//   fromArray?: boolean;
// }

// @Component({
//   selector: 'app-query-container',
//   template: `
//     <ng-container *ngIf="!alternative">
//       <app-side-modal [label]="label" [(value)]="modalVisible" [hideSubmit]="true">
//         <mat-card-content class="h-full overflow-auto">
//           <div class="py-4 flex flex-col">
//             <ng-container [ngTemplateOutlet]="contentDirective.templateRef"></ng-container>
//           </div>
//         </mat-card-content>
//         <mat-divider *ngIf="queriesAvailability()"></mat-divider>
//         <mat-card-content class="flex h-16" *ngIf="queriesAvailability()">
//           <div class="py-4 flex">
//             <app-query-removal />
//           </div>
//         </mat-card-content>
//       </app-side-modal>
//       <div class="flex flex-wrap justify-between items-center md:h-20" *ngIf="!hideFreeTextSearch || !hideFilterBtn">
//         <div class="w-1/2 flex justify-start">
//           <app-free-text-search
//             *ngIf="!hideFreeTextSearch"
//             label="shared.query_engine.search"
//             [standalone]="true"
//             [debounce]="500"
//             [(model)]="freeTextSearch"
//             (modelChange)="onSearchChange()"></app-free-text-search>
//         </div>
//         <div class="w-1/2 space-x-2 flex justify-end" *ngIf="!hideFilterBtn">
//           <app-query-removal />
//           <button mat-button (click)="openModal()">
//             <mat-icon>filter_alt</mat-icon>
//             {{ 'shared.query_engine.filter' | transloco }}
//           </button>
//         </div>
//       </div>
//       <mat-chip-row class="mb-4 mr-2" *ngFor="let filter of appliedFilters" (removed)="removeFilter(filter)">
//         {{ filter.label | transloco }} : {{ filter.text }}
//         <button matChipRemove>
//           <mat-icon>cancel</mat-icon>
//         </button>
//       </mat-chip-row>
//     </ng-container>

//     <ng-container *ngIf="alternative">
//       <div class="flex flex-col">
//         <ng-container *ngIf="contentDirective" [ngTemplateOutlet]="contentDirective.templateRef"></ng-container>
//       </div>
//     </ng-container>
//   `,
// })
// export class QueryContainerComponent<TQueryForm extends QueryForm, TQueryResultItem, TQueryFormParameters>
//   implements OnInit, AfterViewInit, OnDestroy
// {
//   private subs = new SubSink();

//   private static readonly queryFormCache: Record<string, { queryForm: QueryForm; appliedFilters: AppliedFilter[] }> =
//     {};

//   @Input() cacheKey: string;

//   @Input() label: string;

//   @Input() alternative: boolean;

//   @Input() table: TableComponent<TQueryResultItem>;

//   @Input() model: TQueryResultItem[];

//   @Input() hideFreeTextSearch: boolean = false;

//   @Input() hideFilterBtn: boolean = false;

//   @Output() modelChange = new EventEmitter<TQueryResultItem[]>();

//   @Output() dataChange = new EventEmitter<QueryResult<TQueryResultItem>>();

//   @Output() queryPrepareEvent = new EventEmitter<QueryPrepareEvent<TQueryForm>>();

//   @Output() queryEvent = new EventEmitter<QueryEvent<TQueryForm>>();

//   @Output() queryFormEvent = new EventEmitter<QueryFormEvent<TQueryFormParameters>>();

//   @Output() clientQueryEvent = new EventEmitter<QueryClientSourceQueryEvent<TQueryForm, TQueryResultItem>>();

//   properties = new ReplaySubject<Record<string, QueryFormPropertyType>>();

//   properties$ = this.properties.asObservable();

//   queryForm = new BehaviorSubject<TQueryForm>({} as TQueryForm);

//   queryForm$ = this.queryForm.asObservable();

//   filters = new BehaviorSubject<QueryFilters<TQueryForm>>({} as QueryFilters<TQueryForm>);

//   filters$ = this.filters.asObservable();

//   modalVisible = false;

//   freeTextSearch: FreeTextSearchValue;

//   appliedFilters: AppliedFilter[] = [];

//   private defaultFilters: AppliedFilter[] = [];

//   private isInitialQuery = true;

//   private originalData: TQueryResultItem[] = [];

//   originalFilters: QueryFilters<TQueryForm> = {} as QueryFilters<TQueryForm>;

//   @ContentChild(QueryContainerContentDirective) contentDirective: QueryContainerContentDirective;

//   constructor(
//     //private readonly notificationService: NotificationService,
//     private readonly client: QueryEngineClient<TQueryForm, TQueryResultItem, TQueryFormParameters>,
//   ) {}

//   ngOnInit(): void {
//     if (this.cacheKey && QueryContainerComponent.queryFormCache[this.cacheKey]) {
//       this.queryForm.next(QueryContainerComponent.queryFormCache[this.cacheKey].queryForm as TQueryForm);
//       this.appliedFilters = QueryContainerComponent.queryFormCache[this.cacheKey].appliedFilters;
//     }

//     if (this.alternative) {
//       this.queryFormEvent.emit({
//         execute: (path, params) => this.triggerQueryForm(path, params),
//       });
//     }
//   }

//   ngAfterViewInit(): void {
//     this.subs.sink = this.table.loadEvent.subscribe((event) => this.preprareThenBatch(event));
//     this.subs.sink = this.table.pageEvent.subscribe((event) => this.preprareThenBatch(event));
//   }

//   private preprareThenBatch(event: TablePageEvent) {
//     this.queryPrepareEvent.emit({
//       query: { ...this.queryForm.value },
//       isInitialQuery: this.isInitialQuery,
//       updateQuery: (name, model, label, text, isDefault) => {
//         if (isDefault) {
//           this.defaultFilters.push({
//             name: name,
//             label: label,
//             value: model,
//             text: text,
//           });
//         }

//         this.updateQuery(name, model, label, text, true);
//       },
//     });

//     if (this.isInitialQuery) {
//       this.isInitialQuery = false;
//     }

//     this.mergeQuery(event);
//   }

//   ngOnDestroy(): void {
//     this.subs.unsubscribe();
//     this.properties.complete();
//     this.queryForm.complete();
//     this.filters.complete();

//     if (this.cacheKey) {
//       QueryContainerComponent.queryFormCache[this.cacheKey] = {
//         queryForm: this.queryForm.value,
//         appliedFilters: this.appliedFilters,
//       };
//     }
//   }

//   openModal() {
//     this.modalVisible = true;

//     this.queryFormEvent.emit({
//       execute: (path, params) => this.triggerQueryForm(path, params),
//     });
//   }

//   onSearchChange() {
//     this.updateTable();
//   }

//   removeFilter(filter: AppliedFilter) {
//     const newValue = { ...this.queryForm.value };

//     // this code is very messy, refactor this later
//     if (filter.fromArray && Array.isArray(newValue[filter.name]?.values)) {
//       newValue[filter.name].values = (newValue[filter.name].values as Array<any>).filter(
//         (singleValue) => singleValue != filter.value,
//       );
//       this.appliedFilters = this.appliedFilters.filter(
//         (i) => i.name != filter.name || (i.name == filter.name && i.value != filter.value),
//       );

//       if (newValue[filter.name].values.length == 0) {
//         newValue[filter.name] = undefined;
//         this.appliedFilters = this.appliedFilters.filter((i) => i.name != filter.name);
//       }
//     } else {
//       newValue[filter.name] = undefined;
//       this.appliedFilters = this.appliedFilters.filter((i) => i.name != filter.name);
//     }

//     this.queryForm.next(newValue);

//     this.updateTable();
//   }

//   queriesAvailability() {
//     const filters = this.appliedFilters.filter((i) => !this.defaultFilters.some((j) => j.name == i.name));
//     return filters.length > 0 || !!this.freeTextSearch;
//   }

//   clearAllQueries() {
//     this.appliedFilters = this.defaultFilters.slice();
//     this.freeTextSearch = undefined;

//     const newQueryForm = this.defaultFilters.reduce((form, filter) => {
//       form[filter.name] = filter.value;
//       return form;
//     }, {} as TQueryForm);

//     this.queryForm.next(newQueryForm);
//     this.updateTable();
//   }

//   private triggerQueryForm(path: string, params: TQueryFormParameters) {
//     this.subs.sink = this.client.getQueryFormInfo(path, params).subscribe((res) => {
//       this.originalFilters = res.data.filters;
//       this.filters.next(res.data.filters);
//       this.properties.next(res.data.properties);
//       console.log('triggerQueryForm');
//       console.log(this.filters);
//     });
//   }

//   updateQuery(name: string, model: any, label: string, text: string | string[], silent: boolean = false): void {
//     this.queryForm.next({
//       ...this.queryForm.value,
//       ...{ [name]: model },
//     });

//     this.clearAppliedFilter(name);

//     if (Array.isArray(model?.values)) {
//       this.appliedFilters.push(
//         ...model.values.map((singleValue, index) => ({
//           name: name,
//           label: label,
//           value: singleValue,
//           text: text[index],
//           fromArray: true,
//         })),
//       );
//     } else if (model) {
//       this.appliedFilters.push({
//         name: name,
//         label: label,
//         value: model,
//         text: text,
//       });
//     }

//     if (!silent) {
//       this.updateTable();
//     }
//   }

//   clearAppliedFilter(name: string): void {
//     this.appliedFilters = this.appliedFilters.filter((i) => i.name != name);
//   }

//   private mergeQuery(event: TablePageEvent) {
//     let initialQuery = { ...this.queryForm.value };

//     if (!event.isClientSource) {
//       const order: Order =
//         event.sortColumn && event.sortDirection
//           ? {
//               name: event.sortColumn,
//               descending: event.sortDirection == 'desc',
//             }
//           : undefined;

//       const take = event.pageSize;
//       const skip = event.pageSize * event.pageIndex;

//       initialQuery = {
//         ...this.queryForm.value,
//         ...{
//           order: order,
//           take: take,
//           skip: skip,
//           search: this.freeTextSearch,
//         },
//       };
//     }

//     this.queryEvent.emit({
//       query: initialQuery,
//       execute: (path, query) => this.triggerQuery(path, query),
//       executeWithResponseModification: (path, query, modifyResponseFunc) =>
//         this.triggerQuery(path, query, modifyResponseFunc),
//     });
//   }

//   private updateTable() {
//     if (this.table.useClientDataSource) {
//       this.triggerClientQuery();
//     } else {
//       this.table.refresh();
//     }
//   }

//   private triggerQuery(path: string, query: TQueryForm, modifyResponseFunc = null) {
//     this.subs.sink = this.client.query(path, query).subscribe((res) => {
//       if (res.isSuccess) {
//         if (modifyResponseFunc) {
//           modifyResponseFunc(res.data.items);
//         }

//         this.dataChange.emit(res.data);
//         this.originalData = res.data.items;

//         this.model = res.data.items;
//         this.modelChange.emit(this.model);
//         this.table.setPaginatorLength(res.data.total);
//       } else {
//         //this.notificationService.showError(res.message);
//       }
//     });
//   }

//   private triggerClientQuery() {
//     this.clientQueryEvent.emit({
//       query: { ...this.queryForm.value, ...{ search: this.freeTextSearch } },
//       data: this.originalData.slice(),
//       apply: (filteredData) => this.applyClientQuery(filteredData),
//     });
//   }

//   private applyClientQuery(filteredData: TQueryResultItem[]) {
//     this.model = filteredData;
//     this.modelChange.emit(this.model);
//     this.table.setPaginatorLength(filteredData.length);
//   }
// }
