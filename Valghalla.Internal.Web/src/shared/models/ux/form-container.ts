import { UntypedFormGroup } from '@angular/forms';

export class FormContainer {
  formGroup: UntypedFormGroup;
  questions: any[];
  title: string;
  readonly: boolean;

  /**
   *
   */
  constructor() {
    this.questions = [];
  }
}
