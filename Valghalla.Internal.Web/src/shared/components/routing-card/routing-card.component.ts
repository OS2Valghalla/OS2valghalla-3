import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-routing-card',
  templateUrl: './routing-card.component.html',
  styleUrls: ['./routing-card.component.scss'],
})
export class RoutingCardComponent {
  @Input() title: string;

  @Input() bodyMessage: string;

  @Input() link: string;

  @Input() icon: string;
}
