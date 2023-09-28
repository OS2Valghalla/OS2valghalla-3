import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: '[appWizardStep]',
})
export class WizardStepDirective {
  constructor(public templateRef: TemplateRef<unknown>) {}
}
