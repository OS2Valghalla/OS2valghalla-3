import { BehaviorSubject } from 'rxjs';
import { FreeTextSearchValue } from './free-text-search-value';
import { Order } from './order';
import { QueryFilters } from './query-form-info';

export interface QueryForm {
  order?: Order;
  search?: FreeTextSearchValue;
  take?: number;
  skip?: number;
}

export interface QueryEvent<TQueryForm extends QueryForm> {
  query: TQueryForm;
  execute(path: string, query: TQueryForm): void;
  executeWithResponseModification(path: string, query: TQueryForm, callback): void;
}

export interface QueryFormEvent<TQueryFormParameters> {
  execute(path: string, params: TQueryFormParameters): void;
}

export interface QueryPrepareEvent<TQueryForm extends QueryForm> {
  query: TQueryForm;
  isInitialQuery: boolean;
  updateQuery(name: string, model: any, label: string, text: string | string[], isDefault: boolean): void;
}

export interface QueryClientSourceQueryEvent<TQueryForm extends QueryForm, T> {
  query: TQueryForm;
  data: T[];
  apply(filteredData: T[]): void;
}

export interface QueryChangeEvent<TQueryForm extends QueryForm, PropertyName extends keyof TQueryForm> {
  form: BehaviorSubject<TQueryForm>;
  filters: BehaviorSubject<QueryFilters<TQueryForm>>;
  originalFilters: QueryFilters<TQueryForm>;
  model: TQueryForm[PropertyName];
}
