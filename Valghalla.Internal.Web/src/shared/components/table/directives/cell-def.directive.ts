import { Directive, Input, TemplateRef } from '@angular/core';
import { TableTypedDirective } from './table-context.directive';

export interface TableCellContext<T> {
  $implicit: T;
  index: number;
  pageIndex: number;
  pageSize: number;
}

@Directive({
  selector: '[appCellDef]',
})
export class TableCellDefinitionDirective<T> implements TableTypedDirective<T> {
  /** This input property will be empty in runtime. Apply for TypeScript typing inference */
  @Input('appCellDef') typing: T;

  constructor(public templateRef: TemplateRef<unknown>) { }

  static ngTemplateContextGuard<T>(
    directive: TableCellDefinitionDirective<T>,
    context: unknown): context is TableCellContext<T> {
    return true;
  }
}