import { AfterContentInit, Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { FormGroupDirective } from './form-group.directive';

@Directive({ selector: '[appFormLabel]' })
export class FormLabelDirective implements OnInit, AfterContentInit {
  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    private readonly formGroup: FormGroupDirective,
  ) {}

  ngOnInit(): void {
    this.addClass('form-label');
  }

  ngAfterContentInit(): void {
    if (this.formGroup.formInput) {
      this.setAttribute('for', this.formGroup.formInput.id);
    } else if (this.formGroup.formSelect) {
      this.setAttribute('for', this.formGroup.formSelect.id);
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
