import { Directive, Input, TemplateRef } from '@angular/core';
import { TableTypedDirective } from './table-context.directive';

export interface TableHeaderCellContext<T> {
  $implicit: T;
}

@Directive({
  selector: '[appHeaderCellDef]',
})
export class TableHeaderCellDefinitionDirective<T> implements TableTypedDirective<T> {
  /** This input property will be empty in runtime. Apply for TypeScript typing inference */
  @Input('appHeaderCellDef') typing: T;

  /** Make this column sortable */
  @Input() appHeaderCellDefSortable = false;

  constructor(public templateRef: TemplateRef<unknown>) { }

  static ngTemplateContextGuard<T>(
    directive: TableHeaderCellDefinitionDirective<T>,
    context: unknown): context is TableHeaderCellContext<T> {
    return true;
  }
}