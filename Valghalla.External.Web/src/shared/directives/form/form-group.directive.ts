import { ContentChild, Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { FormLabelDirective } from './form-label.directive';
import { FormHintDirective } from './form-hint.directive';
import { FormErrorDirective } from './form-error.directive';
import { FormInputDirective } from './form-input.directive';
import { FormSelectDirective } from './form-select.directive';
import { FormRadioOptionDirective } from './form-radio-option.directive';

@Directive({ selector: '[appFormGroup]' })
export class FormGroupDirective implements OnInit {
  @ContentChild(FormInputDirective) formInput: FormInputDirective;
  @ContentChild(FormSelectDirective) formSelect: FormSelectDirective;
  @ContentChild(FormRadioOptionDirective) formRadioOption: FormRadioOptionDirective;
  @ContentChild(FormLabelDirective) formLabel: FormLabelDirective;
  @ContentChild(FormHintDirective) formHint: FormHintDirective;
  @ContentChild(FormErrorDirective) formError: FormErrorDirective;

  constructor(private readonly renderer: Renderer2, private readonly elementRef: ElementRef) {}

  ngOnInit(): void {
    this.addClass('form-group');
  }

  addClass(name: string) {
    this.renderer.addClass(this.elementRef.nativeElement, name);
  }

  removeClass(name: string) {
    this.renderer.removeClass(this.elementRef.nativeElement, name);
  }
}
