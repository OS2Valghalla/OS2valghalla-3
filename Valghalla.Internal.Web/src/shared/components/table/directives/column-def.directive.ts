import { ContentChild, Directive, Input } from '@angular/core';
import { TableCellDirective } from './cell.directive';
import { TableCellDefinitionDirective } from './cell-def.directive';
import { TableHeaderCellDefinitionDirective } from './header-cell-def.directive';
import { TableTypedDirective } from './table-context.directive';

@Directive({
  selector: '[appColumnDef]',
})
export class TableColumnDefinitionDirective<T> implements TableTypedDirective<T> {
  public name: string;
  private _columnWidth: string = null;

  /** This input property will be empty in runtime. Apply for TypeScript typing inference */
  // eslint-disable-next-line @angular-eslint/no-input-rename
  @Input('appTableTyping') typing: T;

  /** Column identifier */
  @Input() set appColumnDef(value: keyof T | any) {
    this.name = value as string;
  }

  @Input()
  set columnWidth(value: number | string) {
    this._columnWidth = typeof value == 'number' ? value + 'px' : value;
  }

  get columnWidth() {
    return this._columnWidth;
  }

  @ContentChild(TableHeaderCellDefinitionDirective, { descendants: false })
  headerCellDef: TableHeaderCellDefinitionDirective<T>;

  @ContentChild(TableCellDefinitionDirective, { descendants: false }) cellDef: TableCellDefinitionDirective<T>;

  @ContentChild(TableCellDirective, { descendants: false }) cell?: TableCellDirective<T>;
}
