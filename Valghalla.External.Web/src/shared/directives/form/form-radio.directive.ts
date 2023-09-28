import { ContentChild, Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { FormRadioOptionDirective } from './form-radio-option.directive';

@Directive({ selector: '[appFormRadio]' })
export class FormRadioDirective implements OnInit {
  @ContentChild(FormRadioOptionDirective) formRadioOption: FormRadioOptionDirective;

  constructor(private readonly renderer: Renderer2, private readonly elementRef: ElementRef) {}

  ngOnInit(): void {
    this.addClass('form-group-checkbox');
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
