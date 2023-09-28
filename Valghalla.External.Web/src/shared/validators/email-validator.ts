import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function emailValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null;
    }

    const valid = /.+@.+\.[\w\d]+/.test(value);

    return valid ? null : { emailInvalid: true };
  };
}
