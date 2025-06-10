import { SelectionModel } from '@angular/cdk/collections';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import {
  AfterContentInit,
  AfterViewInit,
  Component,
  ContentChild,
  ContentChildren,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, SortDirection } from '@angular/material/sort';
import { combineLatest } from 'rxjs';
import {
  CustomActionDefinition,
  TableDeleteRowEvent,
  TableEditRowEvent,
  TablePageEvent,
  TableSelectRowsEvent,
} from 'src/shared/models/ux/table';
import { QueryContainerComponent } from 'src/shared/query-engine/components/query-container.component';
import { SubSink } from 'subsink';
import { TableColumnDefinitionDirective } from './directives/column-def.directive';
import { TableHeaderRowDefinitionDirective, VisibleColumns } from './directives/header-row-def.directive';
import { TableContextDirective } from './directives/table-context.directive';
import { TablePanelDefinitionDirective } from './directives/table-panel-def.directive';
import { ServerDataSource, TableDataSource, updateDataSource } from './table-data-source';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class TableComponent<T> implements OnInit, OnDestroy, AfterViewInit, AfterContentInit {
  private subs = new SubSink();

  /** Define page size options for table. The first value will be initial page size */
  @Input() pageSizeOptions: Array<number> = [10, 25, 50];
  @Input() pageSize: number = 10;

  @Input() onlyOneSelection = false;

  @Input() defaultSortColumn: string;
  @Input() defaultSortDirection: SortDirection = 'asc';

  /** For panel size */
  @Input() tablePanelSize: string = 'w-1/4';
  @Input() mainPanelSize: string = 'w-3/4';

  /** Define session storage key table size settings and current page number */
  private _storageKey: string = 'app-table';

  @Input()
  set storageKey(value: string) {
    this._storageKey = this.slugifyKey(value);
  }
  get storageKey(): string {
    return this._storageKey;
  }

  private slugifyKey(value: string): string {
    if (!value) return 'app-table';
    const slug = value
      .toString()
      .trim()
      .toLowerCase()
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/^-+|-+$/g, '');
    return `app-table-${slug}`;
  }

  @Input() rowNgClass: (row: T) => any;

  /** Page event trigger. This will be called when paginator or sort changed */
  @Output() pageEvent = new EventEmitter<TablePageEvent>();

  /** Load event trigger. This will be called the first time table render */
  @Output() loadEvent = new EventEmitter<TablePageEvent>();

  /** Edit event trigger. This will be called when clicking edit button in a row */
  @Output() editEvent = new EventEmitter<TableEditRowEvent<T>>();

  /** Delete event trigger. This will be called when clicking delete button in a row */
  @Output() deleteEvent = new EventEmitter<TableDeleteRowEvent<T>>();

  /** Select event trigger. This will be called when toggle selection button in a row or select all button */
  @Output() selectEvent = new EventEmitter<TableSelectRowsEvent<T>>();

  @ViewChild(MatPaginator) readonly paginator: MatPaginator;

  @ViewChild(MatSort) readonly sort: MatSort;

  @ContentChildren(TableColumnDefinitionDirective, { descendants: false })
  readonly columnDefs!: QueryList<TableColumnDefinitionDirective<T>>;

  @ContentChild(TableHeaderRowDefinitionDirective, { descendants: false })
  readonly headerRowDef!: TableHeaderRowDefinitionDirective<T>;

  @ContentChild(QueryContainerComponent, { descendants: false })
  readonly queryContainer: QueryContainerComponent<unknown, T, unknown>;

  @ContentChild(TablePanelDefinitionDirective, { descendants: false })
  readonly tablePanelDef: TablePanelDefinitionDirective<T>;

  readonly selection = new SelectionModel<keyof T>(true, []);

  keys: Array<keyof T> = [];

  dataSource: TableDataSource<T>;

  dataLoaded: boolean = false;

  visibleColumns: Array<VisibleColumns<T>> = [];

  customActions: Array<CustomActionDefinition> = [];

  panelRow: T;

  panelVisible: boolean = false;

  private isMobileView: boolean = false;

  constructor(
    private readonly breakpointObserver: BreakpointObserver,
    public readonly tableContext: TableContextDirective<Array<T>>,
  ) { }

  ngOnInit(): void {
    this.dataSource = new ServerDataSource();
    this.subs.sink = this.tableContext.data$.subscribe((data) => {
      updateDataSource(this.dataSource, data);
      this.dataLoaded = true;
    });

    this.subs.sink = this.selection.changed.subscribe((selectionChange) => {
      this.selectEvent.emit({
        selectedRows: selectionChange.source.selected,
        addedRows: selectionChange.added,
        removedRows: selectionChange.removed,
      });
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngAfterContentInit(): void {
    if (this.queryContainer && !this.queryContainer.table) {
      this.queryContainer.table = this;
    }

    this.subs.sink = combineLatest({
      breakpointState: this.breakpointObserver.observe([
        Breakpoints.XSmall,
        Breakpoints.Small,
        Breakpoints.Medium,
        Breakpoints.Large,
        Breakpoints.XLarge,
      ]),
      columns: this.headerRowDef.columnsChange$,
      mobileColumns: this.headerRowDef.mobileColumnsChange$,
      customActions: this.headerRowDef.customActionsChange$,
    }).subscribe((value) => {
      this.customActions = value.customActions;
      this.isMobileView =
        value.breakpointState.breakpoints[Breakpoints.Small] || value.breakpointState.breakpoints[Breakpoints.XSmall];

      if (this.isMobileView) {
        this.panelVisible = false;
        this.panelRow = undefined;
        this.visibleColumns = value.mobileColumns;
      } else if (this.panelVisible) {
        return;
      } else {
        this.visibleColumns = value.columns;
      }
    });
  }

  ngAfterViewInit(): void {
    const savedPageSize = +sessionStorage.getItem(`${this.storageKey}-pageSize`);
    const savedPageIndex = +sessionStorage.getItem(`${this.storageKey}-pageIndex`);
    if (savedPageSize) {
      this.paginator.pageSize = savedPageSize;
    }

    if (!isNaN(savedPageIndex)) {
      this.paginator.pageIndex = savedPageIndex;
    }
    this.refresh();

    this.subs.sink = this.paginator.page.subscribe((event) => {
      sessionStorage.setItem(`${this.storageKey}-pageSize`, event.pageSize.toString());
      sessionStorage.setItem(`${this.storageKey}-pageIndex`, event.pageIndex.toString());
      this.createMockData();

      this.pageEvent.emit({
        pageSize: event.pageSize,
        pageIndex: event.pageIndex,
        sortColumn: this.sort.active,
        sortDirection: this.sort.direction,
      });
    });

    this.subs.sink = this.sort.sortChange.subscribe((event) => {
      this.paginator.firstPage();
      this.createMockData();

      this.pageEvent.emit({
        pageSize: this.paginator.pageSize,
        pageIndex: 0,
        sortColumn: event.active,
        sortDirection: event.direction,
      });
    });
  }

  closePanel() {
    this.panelVisible = false;

    setTimeout(() => {
      this.panelRow = undefined;
      this.visibleColumns = this.isMobileView ? this.headerRowDef.mobileColumns : this.headerRowDef.columns;
    }, 500);
  }

  refresh() {
    const savedPageIndex = +sessionStorage.getItem(`${this.storageKey}-pageIndex`);
    if (isNaN(savedPageIndex)) {
      this.paginator.firstPage();
    }
    this.selection.clear();

    this.createMockData();

    this.loadEvent.emit({
      pageSize: this.paginator.pageSize,
      pageIndex: this.paginator.pageIndex ?? 0,
      sortColumn: this.sort.active,
      sortDirection: this.sort.direction,
    });
  }

  setPaginator(keys: Array<keyof T>) {
    this.keys = keys;
    this.paginator.length = keys.length;
  }

  isAllSelected() {
    return this.keys.every((key) => this.selection.selected.some((currentKey) => currentKey == key));
  }

  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.clear();
    this.selection.select(...this.keys);
  }

  onRowSelecting(row) {
    if (this.onlyOneSelection) {
      const isSelecting = this.selection.isSelected(row[this.headerRowDef.key]);
      this.selection.clear();
      if (!isSelecting) {
        this.selection.select(row[this.headerRowDef.key]);
      }
    } else {
      this.selection.toggle(row[this.headerRowDef.key]);
    }
  }

  onEditRow(row: T, index: number) {
    this.editEvent.emit({
      row: row,
      index: index,
    });
  }

  onDeleteRow(row: T, index: number) {
    this.deleteEvent.emit({
      row: row,
      index: index,
    });
  }

  onCellClick(columnRef: TableColumnDefinitionDirective<T>, row: T, index: number) {
    columnRef.cell?.cellClick?.emit({
      row: row,
      index: index,
      column: columnRef.name,
    });
  }

  toolTipContent(value) {
    if (typeof value == 'boolean') {
      return null;
    } else if (typeof value == 'object') {
      return null;
    } else return value;
  }

  mergeStyleObj(objectList) {
    const obj = {};
    objectList.forEach(function (x) {
      for (const i in x) obj[i] = x[i];
    });
    return obj;
  }

  createMockData() {
    this.dataLoaded = false;
    updateDataSource(this.dataSource, new Array(10).fill({}));
  }
}
