import { AfterContentInit, Directive, ElementRef } from '@angular/core';
import { datePicker } from 'dkfds';

@Directive({ selector: '[appDatePicker]' })
export class DatePickerDirective implements AfterContentInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngAfterContentInit(): void {
    datePicker.on(this.elementRef.nativeElement);
  }
}
