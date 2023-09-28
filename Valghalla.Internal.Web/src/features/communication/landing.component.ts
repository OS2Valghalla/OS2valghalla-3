import { Component } from '@angular/core';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-communication-landing',
  templateUrl: './landing.component.html',
})
export class CommunicationLandingComponent {
  readonly routingNodes = [
    {
      routerLink: RoutingNodes.CommunicationTemplates,
      title: 'app.navigation.communication.communication_templates.title',
      icon: 'textsms',
      bodyMessage: 'app.navigation.communication.communication_templates.description',
    },
    {
      routerLink: RoutingNodes.CommunicationLogs,
      title: 'app.navigation.communication.communication_logs.title',
      icon: 'query_stats',
      bodyMessage: 'app.navigation.communication.communication_logs.description',
    },
    {
      routerLink: RoutingNodes.SendMessage,
      title: 'app.navigation.communication.send_message.title',
      icon: 'send',
      bodyMessage: 'app.navigation.communication.send_message.description',
    },
  ];
}
