import { Inject, Injectable, ViewContainerRef } from '@angular/core';
import { TOAST_CONTAINER } from '../constants/injection-token';
import { ReplaySubject, take } from 'rxjs';
import { ToastComponent } from '../components/toast/toast.component';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  constructor(@Inject(TOAST_CONTAINER) private readonly toastContainerInjection: ReplaySubject<ViewContainerRef>) {}

  showSuccess(message: string, heading?: string): void {
    this.toastContainerInjection.pipe(take(1)).subscribe((ref) => {
      this.initializeComponent(ref, 'success', message, heading);
    });
  }

  showError(message: string, heading?: string, parseAsHtml?: boolean): void {
    this.toastContainerInjection.pipe(take(1)).subscribe((ref) => {
      this.initializeComponent(ref, 'error', message, heading, parseAsHtml);
    });
  }

  showWarning(message: string, heading?: string): void {
    this.toastContainerInjection.pipe(take(1)).subscribe((ref) => {
      this.initializeComponent(ref, 'warning', message, heading);
    });
  }

  showInfo(message: string, heading?: string): void {
    this.toastContainerInjection.pipe(take(1)).subscribe((ref) => {
      this.initializeComponent(ref, 'info', message, heading);
    });
  }

  private initializeComponent(
    ref: ViewContainerRef,
    type: 'success' | 'warning' | 'error' | 'info',
    message: string,
    heading?: string,
    parseAsHtml?: boolean
  ) {
    const component = ref.createComponent(ToastComponent);
    component.instance.type = type;
    component.instance.message = message;
    component.instance.heading = heading;
    component.instance.parseAsHtml = parseAsHtml;
    component.instance.onHide.pipe(take(1)).subscribe(() => {
      component.destroy();
    });

    component.changeDetectorRef.detectChanges();

    component.instance.show();
  }
}
