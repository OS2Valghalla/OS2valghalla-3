import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: '[appQueryContainerContent]',
})
export class QueryContainerContentDirective {
  constructor(readonly templateRef: TemplateRef<unknown>) {}
}
