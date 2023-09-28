import { Directive, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appDynamicQueryContent]',
})
export class DynamicQueryContentDirective<T> {
  $implicit: T;

  constructor(
    readonly templateRef: TemplateRef<unknown>,
    readonly viewContainerRef: ViewContainerRef,
  ) {}

  static ngTemplateContextGuard<T extends Array<any>>(
    directive: DynamicQueryContentDirective<T>,
    context: unknown,
  ): context is { $implicit: T } {
    return true;
  }
}
