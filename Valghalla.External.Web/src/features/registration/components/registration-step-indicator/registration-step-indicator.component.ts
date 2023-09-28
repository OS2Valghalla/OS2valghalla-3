import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-registration-step-indicator',
  templateUrl: './registration-step-indicator.component.html',
})
export class RegistrationStepIndicatorComponent implements OnInit {
  @Input() type: 'team' | 'task';
  @Input() currentStep: number;

  readonly steps: string[] = [];

  ngOnInit(): void {
    if (this.type == 'team') {
      this.steps.push(
        ...[
          'registration.label.steps.disclosure_statement',
          'registration.label.steps.profile_input',
          'registration.label.steps.user_creation',
        ],
      );
    } else if (this.type == 'task') {
      this.steps.push(
        ...[
          'registration.label.steps.disclosure_statement',
          'registration.label.steps.profile_input',
          'registration.label.steps.user_creation',
          'registration.label.steps.receipt_for_registration',
        ],
      );
    }
  }
}
