import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { ToastDirective } from 'src/shared/dkfds/directives/toast.directive';

@Component({
  selector: '[appToastComponent]',
  templateUrl: './toast.component.html',
})
export class ToastComponent {
  readonly id = crypto.randomUUID();

  @Input() type: 'success' | 'warning' | 'error' | 'info';
  @Input() heading: string;
  @Input() message: string;
  @Input() parseAsHtml: boolean;

  @Output() onHide = new EventEmitter<void>();
  @ViewChild('toastDirective', { read: ToastDirective }) toastDirective: ToastDirective;

  show() {
    this.toastDirective.show();
  }

  hide() {
    this.toastDirective.hide();
    this.onHide.emit();
  }
}
