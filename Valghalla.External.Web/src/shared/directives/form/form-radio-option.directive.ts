import { Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { FormControlDirective as NgFormControlDirective } from '@angular/forms';
import { FormControlDirective } from './standalone/form-control.directive';
import { FormRadioGroupDirective } from './form-radio-group.directive';

@Directive({ selector: '[appFormRadioOption]', hostDirectives: [FormControlDirective] })
export class FormRadioOptionDirective implements OnInit {
  readonly id = crypto.randomUUID();

  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    private readonly formRadioGroup: FormRadioGroupDirective,
    public readonly ngFormControl: NgFormControlDirective,
  ) {}

  ngOnInit(): void {
    this.setAttribute('id', this.id);
    this.setAttribute('name', this.formRadioGroup.id);
    this.addClass('form-radio');
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
