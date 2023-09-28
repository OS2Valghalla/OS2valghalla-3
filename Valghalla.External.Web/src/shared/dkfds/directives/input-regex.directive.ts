import { Directive, ElementRef, OnInit } from '@angular/core';
import { InputRegexMask } from 'dkfds';

@Directive({ selector: '[appInputRegex]' })
export class InputRegexDirective implements OnInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngOnInit(): void {
    new InputRegexMask(this.elementRef.nativeElement);
  }
}
