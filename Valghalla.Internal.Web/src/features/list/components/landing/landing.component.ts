import { Component } from '@angular/core';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-list-landing',
  templateUrl: './landing.component.html',
})
export class ListLandingComponent {
  readonly routingNodes = [
    {
        routerLink: RoutingNodes.ParticipantList,
        title: 'app.navigation.list.participant_list.title',
        icon: 'list_alt',
        bodyMessage: 'app.navigation.list.participant_list.description',
    },
    {
      routerLink: RoutingNodes.ElectionSystemList,
      title: 'app.navigation.list.election_system_list.title',
      icon: 'list_alt',
      bodyMessage: 'app.navigation.list.election_system_list.description',
  },
  ];
}
