import { Directive, ElementRef, OnInit } from '@angular/core';
import { ErrorSummary } from 'dkfds';

@Directive({ selector: '[appErrorSummary]' })
export class ErrorSummaryDirective implements OnInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngOnInit(): void {
    new ErrorSummary(this.elementRef.nativeElement).init();
  }
}
