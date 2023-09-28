import { Component, Input, ViewChild } from '@angular/core';
import { PageMenuItem } from '../../../shared/models/ux/PageMenuItem';

@Component({
  selector: 'app-page-menu-item',
  templateUrl: './page-menu-item.component.html',
})
export class PageMenuItemComponent {
  @Input() items: PageMenuItem[] = [];

  // tslint:disable-next-line
  @ViewChild('menu', { static: true }) menu: any;
}