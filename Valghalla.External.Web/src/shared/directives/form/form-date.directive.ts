import { Directive } from '@angular/core';
import { FormAriaDescribedByDirective } from './standalone/form-aria-describedby.directive';

@Directive({ selector: '[appFormDate]', hostDirectives: [FormAriaDescribedByDirective] })
export class FormDateDirective {}
