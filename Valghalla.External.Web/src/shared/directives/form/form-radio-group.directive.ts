import { Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { FormAriaDescribedByDirective } from './standalone/form-aria-describedby.directive';
import { FormAriaInvalidDirective } from './standalone/form-aria-invalid.directive';

@Directive({
  selector: '[appFormRadioGroup]',
  hostDirectives: [FormAriaDescribedByDirective, FormAriaInvalidDirective],
})
export class FormRadioGroupDirective implements OnInit {
  readonly id = crypto.randomUUID();
  
  constructor(private readonly renderer: Renderer2, private readonly elementRef: ElementRef) {}

  ngOnInit(): void {
    this.setAttribute('role', 'radiogroup');
  }

  setAttribute(name: string, value: string) {
    this.renderer.setAttribute(this.elementRef.nativeElement, name, value);
  }

  addClass(name: string) {
    this.renderer.addClass(this.elementRef.nativeElement, name);
  }

  removeClass(name: string) {
    this.renderer.removeClass(this.elementRef.nativeElement, name);
  }
}
