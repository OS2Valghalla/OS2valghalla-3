import { Directive, ElementRef, OnInit } from '@angular/core';
import { Toast } from 'dkfds';

@Directive({
  selector: '[appToast]',
  exportAs: 'appToast',
})
export class ToastDirective implements OnInit {
  private instance: any;

  constructor(private readonly elementRef: ElementRef) {}

  ngOnInit(): void {
    this.instance = new Toast(this.elementRef.nativeElement);
  }

  show() {
    this.instance.show();
  }

  hide() {
    this.instance.hide();
  }
}
