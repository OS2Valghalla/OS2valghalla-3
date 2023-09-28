import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-field-value',
  templateUrl: './field-value.component.html',
})
export class FieldValueComponent {
  @Input() fieldLabel: string;
  @Input() linkValue: string;

  constructor(private router: Router) {}

  cleanLink(link: string) {
    link = link.replace('www.', '');
    if (!link.startsWith('https') || !link.startsWith('http')) return 'https://' + link;

    return link;
  }
}
