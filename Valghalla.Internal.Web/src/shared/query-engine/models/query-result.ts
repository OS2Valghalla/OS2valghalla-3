export interface QueryResult<T> extends QueryResultBase<T, string> {}

export interface QueryResultBase<T, K> {
  keys: K[];
  items: T[];
}
