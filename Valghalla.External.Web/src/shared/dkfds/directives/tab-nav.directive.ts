import { Directive, ElementRef, OnInit } from '@angular/core';
import { Tabnav } from 'dkfds';

@Directive({ selector: '[appTabNav]' })
export class TabNavDirective implements OnInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngOnInit(): void {
    new Tabnav(this.elementRef.nativeElement).init();
  }
}
