import { Directive, Input, TemplateRef } from '@angular/core';
import { TableTypedDirective } from './table-context.directive';

export interface TablePanelContext<T> {
  $implicit: T;
}

@Directive({
  selector: '[appTablePanelDef]',
})
export class TablePanelDefinitionDirective<T> implements TableTypedDirective<T> {
  /** This input property will be empty in runtime. Apply for TypeScript typing inference */
  @Input('appTablePanelDef') typing: T;

  constructor(public templateRef: TemplateRef<unknown>) {}

  static ngTemplateContextGuard<T>(
    directive: TablePanelDefinitionDirective<T>,
    context: unknown,
  ): context is TablePanelContext<T> {
    return true;
  }
}
