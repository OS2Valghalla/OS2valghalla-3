import { Component, Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Component({
  template: `
    <div class="spinner"></div>
    <div class="spinner-status" role="status">{{ 'shared.common.loading' | transloco }}</div>
  `,
})
export class SpinnerComponent {}

@Directive({ selector: '[appSpinner]' })
export class SpinnerDirective {
  private hasSpinner = false;

  constructor(private templateRef: TemplateRef<any>, private viewContainer: ViewContainerRef) {}

  @Input() set appSpinner(condition: boolean) {
    if (condition && !this.hasSpinner) {
      this.viewContainer.createComponent(SpinnerComponent);
      this.hasSpinner = true;
    } else {
      if (this.hasSpinner) {
        this.viewContainer.clear();
      }

      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }
}
