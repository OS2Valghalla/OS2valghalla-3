import { Component, Input } from '@angular/core';
import { PageMenuItem } from '../../../shared/models/ux/PageMenuItem';
import { Role } from '../../constants/role';

@Component({
  selector: 'app-page-menu',
  template: `
    <div class="block text-end">
      <button mat-raised-button color="primary" [matMenuTriggerFor]="pagemenu.menu">
        {{ label | transloco }}
      </button>
      <app-page-menu-item #pagemenu [items]="items"></app-page-menu-item>
    </div>
  `,
})
export class PageMenuComponent {
  @Input() label = 'shared.quick_menu.label';
  @Input() items: PageMenuItem[] = [];
  @Input() allowReader = false;
  public role = Role.editor;

  constructor() {}
}
