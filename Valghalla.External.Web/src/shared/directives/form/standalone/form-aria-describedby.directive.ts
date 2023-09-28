import { AfterContentInit, Directive, ElementRef, Renderer2 } from '@angular/core';
import { FormGroupDirective } from '../form-group.directive';

@Directive({ selector: '[appFormAriaDescribedBy]', standalone: true })
export class FormAriaDescribedByDirective implements AfterContentInit {
  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    private readonly formGroup: FormGroupDirective,
  ) {}

  ngAfterContentInit(): void {
    const values = [];

    if (this.formGroup.formHint) {
      values.push(this.formGroup.formHint.id);
    }

    if (this.formGroup.formError) {
      values.push(this.formGroup.formError.id);
    }

    if (values.length > 0) {
      this.setAttribute('aria-describedby', values.join(' '));
    }
  }

  setAttribute(name: string, value: string) {
    this.renderer.setAttribute(this.elementRef.nativeElement, name, value);
  }
}
