export interface TablePageEvent {
  pageSize: number;
  pageIndex: number;
  sortColumn?: string;
  sortDirection?: any;
  isClientSource: boolean;

  /** For server side filter, update number of total items for paginator */
  setTotal(value: number): void;
}

export interface TableClientSortingDataAccessor<T> {
  (data: T, sortColumn: string): string | number;
}

export interface TableEditRowEvent<T> {
  row: T;
  index: number;
}

export interface TableDeleteRowEvent<T> {
  row: T;
  index: number;
}

export interface TableCellClickEvent<T> {
  row: T;
  index: number;
  column: string;
}

export interface TableSelectRowsEvent<T> {
  selectedRows: Array<keyof T>;
  addedRows: Array<keyof T>;
  removedRows: Array<keyof T>;
}
