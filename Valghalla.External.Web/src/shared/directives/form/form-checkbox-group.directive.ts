import { Directive } from '@angular/core';
import { FormAriaDescribedByDirective } from './standalone/form-aria-describedby.directive';

@Directive({
  selector: '[appFormCheckboxGroup]',
  hostDirectives: [FormAriaDescribedByDirective],
})
export class FormCheckboxGroupDirective {
  readonly id = crypto.randomUUID();
}
