import { Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';

@Directive({ selector: '[appFormHint]' })
export class FormHintDirective implements OnInit {
  readonly id = crypto.randomUUID();

  constructor(private readonly renderer: Renderer2, private readonly elementRef: ElementRef) {}

  ngOnInit(): void {
    this.setAttribute('id', this.id);
    this.addClass('form-hint');
  }

  setAttribute(name: string, value: string) {
    this.renderer.setAttribute(this.elementRef.nativeElement, name, value);
  }

  addClass(name: string) {
    this.renderer.addClass(this.elementRef.nativeElement, name);
  }

  removeClass(name: string) {
    this.renderer.removeClass(this.elementRef.nativeElement, name);
  }
}
