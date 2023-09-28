import { Component, ContentChild, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { WizardStepDirective } from './wizard-step.directive';

@Component({
  selector: 'app-wizard-step',
  template: `<ng-content></ng-content>`,
})
export class WizardStepComponent {
  @Input() stepTitle: string;
  
  @Input() formGroup: FormGroup;

  @ContentChild(WizardStepDirective, { descendants: false }) directive: WizardStepDirective;
}
