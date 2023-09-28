import { Component, Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Component({
  template: '<ngx-skeleton-loader [theme]="{height: \'50px\'}"></ngx-skeleton-loader>',
})
export class AppSkeletonComponent {}

@Directive({ selector: '[appShowSkeleton]' })
export class ShowSkeletonDirective {
  private hasSkeleton = false;

  // tslint:disable-next-line
  constructor(private templateRef: TemplateRef<any>, private viewContainer: ViewContainerRef) {}

  @Input() set appShowSkeleton(condition: boolean) {
    if (condition && !this.hasSkeleton) {
      this.viewContainer.createComponent(AppSkeletonComponent);
      this.hasSkeleton = true;
    } else {
      if (this.hasSkeleton) {
        this.viewContainer.clear();
      }

      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }
}
