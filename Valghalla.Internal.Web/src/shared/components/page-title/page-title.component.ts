import { Component } from '@angular/core';

@Component({
  selector: 'app-page-title',
  template: '<h1 class="mt-0 mb-2"><ng-content></ng-content></h1>',
  styleUrls: ['./page-title.component.scss'],
})
export class PageTitleComponent {}
