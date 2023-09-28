import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SubSink } from 'subsink';
import { TableComponent } from './table.component';

@Component({
  selector: 'app-table-row-link',
  template: `
    <div class="group">
      <span
        class="hover:underline hover:cursor-pointer"
        [ngClass]="{ underline: table.panelRow == value }"
        (click)="onClick()">
        <ng-content></ng-content>
      </span>
      <a
        *ngIf="!isMobileView"
        [routerLink]="routerPath"
        class="relative pl-1 opacity-0 group-hover:opacity-100 transition-opacity ease-in delay-500">
        <mat-icon *ngIf="showLinkButton || table.panelRow == value" class="absolute">visibility</mat-icon>
      </a>
    </div>
  `,
})
export class TableRowLinkComponent<T> implements OnInit {
  private readonly subs = new SubSink();

  @Input() value: T;

  @Input() routerPath: string;

  @Input() showLinkButton: boolean = true;

  isMobileView: boolean = false;

  constructor(
    readonly table: TableComponent<T>,
    private readonly breakpointObserver: BreakpointObserver,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.subs.sink = this.breakpointObserver
      .observe([Breakpoints.XSmall, Breakpoints.Small, Breakpoints.Medium, Breakpoints.Large, Breakpoints.XLarge])
      .subscribe((state) => {
        this.isMobileView = state.breakpoints[Breakpoints.Small] || state.breakpoints[Breakpoints.XSmall];
      });
  }

  onClick() {
    if (this.isMobileView || this.table.panelRow == this.value) {
      this.router.navigate([this.routerPath], {
        relativeTo: this.route,
      });

      return;
    }

    this.table.panelVisible = true;
    this.table.visibleColumns = this.table.headerRowDef.panelColumns;
    this.table.panelRow = this.value;
  }
}
