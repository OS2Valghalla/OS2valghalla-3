import { Directive, ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { FormControlDirective as NgFormControlDirective } from '@angular/forms';
import { FormGroupDirective } from '../form-group.directive';
import { SubSink } from 'subsink';
import { FormAriaInvalidDirective } from './form-aria-invalid.directive';

@Directive({ selector: '[appFormControl]', standalone: true, hostDirectives: [FormAriaInvalidDirective] })
export class FormControlDirective implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  get invalid(): boolean {
    return this.ngFormControl.invalid && !this.ngFormControl.pristine;
  }

  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    private readonly ngFormControl: NgFormControlDirective,
    private readonly formGroup: FormGroupDirective,
  ) {}

  ngOnInit(): void {
    this.subs.sink = this.ngFormControl.control.valueChanges.subscribe(() => this.onFormControlChange());
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  setAttribute(name: string, value: string) {
    this.renderer.setAttribute(this.elementRef.nativeElement, name, value);
  }

  private onFormControlChange() {
    if (this.invalid) {
      this.formGroup.addClass('form-error');
      this.formGroup.formError.show(this.ngFormControl.errors);
    } else {
      this.formGroup.removeClass('form-error');
      this.formGroup.formError.hide();
    }
  }
}
