import { Component } from '@angular/core';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
})
export class LandingComponent {
  readonly routingNodes = [
    {
      routerLink: '/' + RoutingNodes.Administration + '/' + RoutingNodes.WorkLocation,
      title: 'app.navigation.work_location.title',
      icon: 'place',
      bodyMessage: 'app.navigation.work_location.description',
    },
    {
      routerLink: '/' + RoutingNodes.Administration + '/' + RoutingNodes.Web,
      title: 'app.navigation.web.title',
      icon: 'public',
      bodyMessage: 'app.navigation.web.description',
    },
    {
      routerLink: '/' + RoutingNodes.Administration + '/' + RoutingNodes.Area,
      title: 'app.navigation.area.title',
      icon: 'map',
      bodyMessage: 'app.navigation.area.description',
    },
    {
      routerLink: '/' + RoutingNodes.Administration + '/' + RoutingNodes.TaskType,
      title: 'app.navigation.task_type.title',
      icon: 'task',
      bodyMessage: 'app.navigation.task_type.description',
    },
    {
      routerLink: '/' + RoutingNodes.Administration + '/' + RoutingNodes.AuditLog,
      title: 'app.navigation.audit_log.title',
      icon: 'query_stats',
      bodyMessage: 'app.navigation.audit_log.description',
    },
    {
      routerLink: '/' + RoutingNodes.Administration + '/' + RoutingNodes.SpecialDiet,
      title: 'app.navigation.specialdiet.title',
      icon: 'category',
      bodyMessage: 'app.navigation.specialdiet.description',
    },
    {
      routerLink: '/' + RoutingNodes.Administration + '/' + RoutingNodes.Teams,
      title: 'app.navigation.teams.title',
      icon: 'group',
      bodyMessage: 'app.navigation.teams.description',
    },
    {
      routerLink: '/' + RoutingNodes.Administration + '/' + RoutingNodes.Election,
      title: 'app.navigation.election.title',
      icon: 'how_to_vote',
      bodyMessage: 'app.navigation.election.description',
    },
    {
      routerLink: '/' + RoutingNodes.Administration + '/' + RoutingNodes.ElectionType,
      title: 'app.navigation.election_type.title',
      icon: 'category',
      bodyMessage: 'app.navigation.election_type.description',
    },
  ];
}
