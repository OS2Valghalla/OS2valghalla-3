import { ViewContainerRef } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';

export interface SideModal {
  viewContainerRef: ViewContainerRef;
  component: MatSidenav;
}