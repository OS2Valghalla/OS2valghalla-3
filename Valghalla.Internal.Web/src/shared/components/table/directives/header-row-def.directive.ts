import { Directive, Input, OnDestroy } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';
import { GlobalStateService } from '../../../../app/global-state.service';
import { Role } from '../../../constants/role';
import { TableTypedDirective } from './table-context.directive';
import { CustomActionDefinition } from '../../../models/ux/table';

export type VisibleColumns<T> = keyof T | '$selection' | '$actionsEditDelete' | '$actionEdit' | '$actionDelete' | '$customActions';

@Directive({
  selector: '[appHeaderRowDef]',
})
export class TableHeaderRowDefinitionDirective<T> implements TableTypedDirective<T>, OnDestroy {
  private subs = new SubSink();
  private readonly customActionsChange = new BehaviorSubject<Array<CustomActionDefinition>>([]);
  customActionsChange$ = this.customActionsChange.asObservable();
  private readonly columnsChange = new BehaviorSubject<Array<VisibleColumns<T>>>([]);
  columnsChange$ = this.columnsChange.asObservable();
  private readonly mobileColumnsChange = new BehaviorSubject<Array<VisibleColumns<T>>>([]);
  mobileColumnsChange$ = this.mobileColumnsChange.asObservable();
  private adminColumns = ['$selection', '$actionsEditDelete', '$actionEdit', '$actionDelete', '$customActions'];

  /** This input property will be empty in runtime. Apply for TypeScript typing inference */
  // eslint-disable-next-line @angular-eslint/no-input-rename
  @Input('appTableTyping') typing: T;

  @Input() key: keyof T;

  /** Custom actions definitions */
  @Input()
  set customActions(value: Array<CustomActionDefinition>) {
    this.customActionsChange.next(value);
  }

  get customActions() {
    return this.customActionsChange.value;
  } 

  /** Visible columns in normal and large screens */
  @Input()
  set columns(value: Array<VisibleColumns<T>>) {
    this.columnsChange.next(value);
  }

  get columns() {
    return this.columnsChange.value;
  }

  /** Visible columns in small screens */
  @Input()
  set mobileColumns(value: Array<VisibleColumns<T>>) {
    this.mobileColumnsChange.next(value);
  }

  get mobileColumns() {
    return this.mobileColumnsChange.value;
  }
  
  @Input() panelColumns: Array<VisibleColumns<T>> = [];

  /** Make header sticky in overflow mode */
  @Input() sticky = true;

  /** Visible action icons for readers */
  @Input() allowReader = false;

  ngOnDestroy(): void {
    this.subs.unsubscribe();
    this.columnsChange.complete();
    this.mobileColumnsChange.complete();
  }

  constructor(private readonly globalStateService: GlobalStateService) {}
}
