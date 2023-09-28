import { ContentChild, Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { FormCheckboxOptionDirective } from './form-checkbox-option.directive';

@Directive({ selector: '[appFormCheckbox]' })
export class FormCheckboxDirective implements OnInit {
  @ContentChild(FormCheckboxOptionDirective) formCheckboxOption: FormCheckboxOptionDirective;

  constructor(private readonly renderer: Renderer2, private readonly elementRef: ElementRef) {}

  ngOnInit(): void {
    this.addClass('form-group-radio');
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
