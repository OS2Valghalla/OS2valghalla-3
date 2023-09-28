import { Directive, ElementRef, OnInit } from '@angular/core';
import { Tooltip } from 'dkfds';

@Directive({ selector: '[appTooltip]' })
export class TooltipDirective implements OnInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngOnInit(): void {
    new Tooltip(this.elementRef.nativeElement).init();
  }
}
