import { Directive, ElementRef, OnInit } from '@angular/core';
import { CharacterLimit } from 'dkfds';

@Directive({ selector: '[appCharacterLimit]' })
export class CharacterLimitDirective implements OnInit {
  constructor(private readonly elementRef: ElementRef) {}
  ngOnInit(): void {
    new CharacterLimit(this.elementRef.nativeElement).init();
  }
}
