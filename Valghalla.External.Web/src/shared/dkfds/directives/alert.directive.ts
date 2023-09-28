import { Directive, ElementRef, OnInit } from '@angular/core';
import { Alert } from 'dkfds';

@Directive({ selector: '[appAlert]' })
export class AlertDirective implements OnInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngOnInit(): void {
    new Alert(this.elementRef.nativeElement).init();
  }
}
