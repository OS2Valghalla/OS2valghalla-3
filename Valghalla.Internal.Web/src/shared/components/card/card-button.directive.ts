import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: '[appCardButton]',
})
export class CardButtonDirective {
  constructor(public templateRef: TemplateRef<unknown>) {}
}
