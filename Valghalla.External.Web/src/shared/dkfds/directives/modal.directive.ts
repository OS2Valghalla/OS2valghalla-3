import { Directive, ElementRef, AfterViewInit } from '@angular/core';
import { Modal } from 'dkfds';

@Directive({ selector: '[appModal]' })
export class ModalDirective implements AfterViewInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngAfterViewInit(): void {
    new Modal(this.elementRef.nativeElement).init();
  }
}
