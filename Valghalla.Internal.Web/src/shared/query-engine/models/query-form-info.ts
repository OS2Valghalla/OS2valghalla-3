import { SelectOption } from './select-option';
import { MultipleSelectionFilterValue } from './multiple-selection-filter-value';
import { SingleSelectionFilterValue } from './single-selection-filter-value';
import { QueryForm } from './query-form';
import { BooleanFilterValue } from './boolean-filter-value';
import { FreeTextSearchValue } from './free-text-search-value';

type OmitNever<T> = { [K in keyof T as T[K] extends never ? never : K]: T[K] };

export enum QueryFormPropertyType {
  Generic = 0,
  Boolean = 1,
  FreeText = 2,
  SingleSelection = 3,
  MutipleSelection = 4,
  DateTime = 5 ,
}

export type QueryFormInfo<TQueryForm extends QueryForm> = {
  properties: Record<string, QueryFormPropertyType>;
  filters: QueryFilters<TQueryForm>;
};

export type QueryFilters<TQueryForm extends QueryForm> = Omit<
  OmitNever<{
    [k in keyof TQueryForm]: TQueryForm[k] extends SingleSelectionFilterValue<infer P>
      ? SelectOption<P>[]
      : TQueryForm[k] extends MultipleSelectionFilterValue<infer P>
      ? SelectOption<P>[]
      : TQueryForm[k] extends FreeTextSearchValue
      ? never
      : TQueryForm[k] extends BooleanFilterValue
      ? never
      : never;
  }>,
  'order' | 'search' | 'take' | 'skip'
>;
