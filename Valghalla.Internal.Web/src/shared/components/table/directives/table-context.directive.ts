import {
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
  OnDestroy,
  ChangeDetectorRef,
  AfterContentInit,
} from '@angular/core';
import { Subject } from 'rxjs';

export interface TableTypingContext<T extends Array<any>> {
  $implicit: T[0];
}

export interface TableTypedDirective<T> {
  typing: T;
}

@Directive({
  selector: '[appTableCtx]',
})
export class TableContextDirective<T> implements OnDestroy, AfterContentInit {
  private hasView = false;

  private readonly data = new Subject<T>();

  data$ = this.data.asObservable();

  /** Data source for the table. The let context is the typing of item array */
  @Input() set appTableCtx(value: T) {
    this.data.next(value);

    if (!this.hasView) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
    }
  }

  constructor(
    private templateRef: TemplateRef<unknown>,
    private viewContainer: ViewContainerRef,
    private changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnDestroy(): void {
    this.data.complete();
  }

  ngAfterContentInit(): void {
    // workaround solution until this is fixed in Angular
    // https://github.com/angular/angular/issues/16388
    this.changeDetectorRef.detectChanges();
  }

  static ngTemplateContextGuard<T extends Array<any>>(
    directive: TableContextDirective<T>,
    context: unknown,
  ): context is TableTypingContext<T> {
    return true;
  }
}
