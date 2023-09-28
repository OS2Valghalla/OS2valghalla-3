import { Directive, ElementRef, OnInit, Renderer2, Input } from '@angular/core';
import { FormControlDirective as NgFormControlDirective, Validators } from '@angular/forms';
import { FormControlDirective } from './standalone/form-control.directive';
import { FormAriaDescribedByDirective } from './standalone/form-aria-describedby.directive';

@Directive({ selector: '[appFormSelect]', hostDirectives: [FormControlDirective, FormAriaDescribedByDirective] })
export class FormSelectDirective implements OnInit {
  readonly id = crypto.randomUUID();

  @Input() enableStyling: boolean = true;

  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    public readonly ngFormControl: NgFormControlDirective,
  ) {}

  ngOnInit(): void {
    this.setAttribute('id', this.id);
    this.setAttribute('name', this.id);

    if (this.enableStyling) {
      this.addClass('form-select');
    }

    if (this.ngFormControl.control.hasValidator(Validators.required)) {
      this.setAttribute('required', '');
    }
  }

  addClass(name: string) {
    this.renderer.addClass(this.elementRef.nativeElement, name);
  }

  removeClass(name: string) {
    this.renderer.removeClass(this.elementRef.nativeElement, name);
  }

  setAttribute(name: string, value: string) {
    this.renderer.setAttribute(this.elementRef.nativeElement, name, value);
  }
}
