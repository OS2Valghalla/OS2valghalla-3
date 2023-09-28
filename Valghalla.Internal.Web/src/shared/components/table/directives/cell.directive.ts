import { AfterContentInit, ChangeDetectorRef, Directive, EventEmitter, Input, Output } from '@angular/core';
import { TableCellClickEvent } from 'src/shared/models/ux/table';

@Directive({
  selector: '[appCell]',
})
export class TableCellDirective<T> implements AfterContentInit {
  /** Apply NgClass to matCell element */
  @Input() cellClass: string | string[] | Set<string> | { [klass: string]: any };

  /** Apply NgStyle to matCell element */
  @Input() cellStyle: { [klass: string]: any };

  @Input() disableTruncate: boolean = false;

  /** Click event for cell */
  @Output() cellClick = new EventEmitter<TableCellClickEvent<T>>();

  constructor(private changeDetectorRef: ChangeDetectorRef) {}

  ngAfterContentInit(): void {
    // workaround solution until this is fixed in Angular
    // https://github.com/angular/angular/issues/16388
    this.changeDetectorRef.detectChanges();
  }
}
