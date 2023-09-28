import { AfterContentInit, Directive, ElementRef, OnDestroy, Renderer2 } from '@angular/core';
import { SubSink } from 'subsink';
import { FormGroupDirective } from '../form-group.directive';
import { FormControlDirective as NgFormControlDirective } from '@angular/forms';

@Directive({ selector: '[appFormAriaInvalid]', standalone: true })
export class FormAriaInvalidDirective implements AfterContentInit, OnDestroy {
  private readonly subs = new SubSink();

  private ngFormControl: NgFormControlDirective;

  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    private readonly formGroup: FormGroupDirective,
  ) {}

  ngAfterContentInit(): void {
    if (this.formGroup.formInput) {
      this.ngFormControl = this.formGroup.formInput.ngFormControl;
    } else if (this.formGroup.formSelect) {
      this.ngFormControl = this.formGroup.formSelect.ngFormControl;
    } else if (this.formGroup.formRadioOption) {
      this.ngFormControl = this.formGroup.formRadioOption.ngFormControl;
    }

    if (this.ngFormControl) {
      this.subs.sink = this.ngFormControl.control.valueChanges.subscribe(() => this.onFormControlChange());
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  setAttribute(name: string, value: string) {
    this.renderer.setAttribute(this.elementRef.nativeElement, name, value);
  }

  private onFormControlChange() {
    if (this.ngFormControl.invalid) {
      this.setAttribute('aria-invalid', 'true');
    } else {
      this.setAttribute('aria-invalid', 'false');
    }
  }
}
