import { Directive, ElementRef } from '@angular/core';
import { Dropdown } from 'dkfds';

@Directive({ selector: '[appOverflowMenu]' })
export class OverflowMenuDirective {
  constructor(private readonly elementRef: ElementRef) {}
  ngOnInit(): void {
    new Dropdown(this.elementRef.nativeElement).init();
  }
}
