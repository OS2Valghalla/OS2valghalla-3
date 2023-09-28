import { AfterContentInit, Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { FormCheckboxDirective } from './form-checkbox.directive';

@Directive({ selector: '[appFormCheckboxLabel]' })
export class FormCheckboxLabelDirective implements OnInit, AfterContentInit {
  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    private readonly formCheckbox: FormCheckboxDirective,
  ) {}

  ngOnInit(): void {
    this.addClass('form-label');
  }

  ngAfterContentInit(): void {
    if (this.formCheckbox.formCheckboxOption) {
      this.setAttribute('for', this.formCheckbox.formCheckboxOption.id);
    }
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
