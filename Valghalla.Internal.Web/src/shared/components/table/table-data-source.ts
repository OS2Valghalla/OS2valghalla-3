import { DataSource } from '@angular/cdk/collections';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { map, Observable, Subject } from 'rxjs';
import { TableClientSortingDataAccessor } from 'src/shared/models/ux/table';

export interface TableDataSource<T> extends DataSource<T> {
  data: Array<T>;

  attach(paginator: MatPaginator, sort: MatSort, customSortingAccessor: TableClientSortingDataAccessor<T>): void;
}

export function updateDataSource<T>(dataSource: TableDataSource<T>, value: T[]) {
  (dataSource as ServerDataSource<T>).update(value);
}

export class ServerDataSource<T> extends DataSource<T> implements TableDataSource<T> {
  readonly sourceType = 'server';

  private readonly _data = new Subject<Array<T>>();

  private readonly data$ = this._data.asObservable().pipe(
    map((value) => {
      this.data = value;
      return value;
    }),
  );

  public data: Array<T>;

  connect(): Observable<Array<T>> {
    return this.data$;
  }

  disconnect(): void {
    this._data.complete();
  }

  update(value: Array<T>) {
    this._data.next(value);
  }

  attach() {}
}
