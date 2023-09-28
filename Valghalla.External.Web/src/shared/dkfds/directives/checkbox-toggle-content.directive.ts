import { Directive, ElementRef, OnInit } from '@angular/core';
import { CheckboxToggleContent } from 'dkfds';

@Directive({ selector: '[appCheckboxToggleContent]' })
export class CheckboxToggleContentDirective implements OnInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngOnInit(): void {
    new CheckboxToggleContent(this.elementRef.nativeElement).init();
  }
}
