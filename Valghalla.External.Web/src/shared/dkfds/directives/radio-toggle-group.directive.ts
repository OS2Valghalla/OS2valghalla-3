import { Directive, ElementRef, OnInit } from '@angular/core';
import { RadioToggleGroup } from 'dkfds';

@Directive({ selector: '[appRadioToggleGroup]' })
export class RadioToggleGroupDirective implements OnInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngOnInit(): void {
    new RadioToggleGroup(this.elementRef.nativeElement).init();
  }
}
