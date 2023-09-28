import { Component, Input, Optional } from '@angular/core';
import { QueryContainerComponent } from './query-container.component';

@Component({
  selector: 'app-query-removal',
  template: `
    <button mat-button (click)="clearQueries()" *ngIf="isVisible()">
      <mat-icon>filter_alt_off</mat-icon>
      {{ 'shared.query_engine.clear_filter' | transloco }}
    </button>
  `,
})
export class QueryRemovalComponent {
  @Input() container: QueryContainerComponent<unknown, unknown, unknown>;

  constructor(@Optional() private hostedContainer: QueryContainerComponent<unknown, unknown, unknown>) {
    if (this.hostedContainer) {
      this.container = hostedContainer;
    }
  }

  clearQueries() {
    this.container.clearAllQueries();
  }

  isVisible() {
    return this.container.queriesAvailability();
  }
}
