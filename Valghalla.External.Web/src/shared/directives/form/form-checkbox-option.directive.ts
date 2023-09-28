import { Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { FormCheckboxGroupDirective } from './form-checkbox-group.directive';

@Directive({ selector: '[appFormCheckboxOption]' })
export class FormCheckboxOptionDirective implements OnInit {
  readonly id = crypto.randomUUID();

  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    private readonly formCheckboxGroup: FormCheckboxGroupDirective,
  ) {}

  ngOnInit(): void {
    this.setAttribute('id', this.id);
    this.setAttribute('name', this.formCheckboxGroup.id);
    this.addClass('form-checkbox');
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
