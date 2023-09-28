import { AfterContentInit, Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { FormRadioDirective } from './form-radio.directive';

@Directive({ selector: '[appFormRadioLabel]' })
export class FormRadioLabelDirective implements OnInit, AfterContentInit {
  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    private readonly formRadio: FormRadioDirective,
  ) {}

  ngOnInit(): void {
    this.addClass('form-label');
  }

  ngAfterContentInit(): void {
    if (this.formRadio.formRadioOption) {
      this.setAttribute('for', this.formRadio.formRadioOption.id);
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
