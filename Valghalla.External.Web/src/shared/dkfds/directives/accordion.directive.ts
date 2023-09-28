import { Directive, ElementRef, OnInit } from '@angular/core';
import { Accordion } from 'dkfds';

@Directive({ selector: '[appAccordion]' })
export class AccordionDirective implements OnInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngOnInit(): void {
    new Accordion(this.elementRef.nativeElement).init();
  }
}
