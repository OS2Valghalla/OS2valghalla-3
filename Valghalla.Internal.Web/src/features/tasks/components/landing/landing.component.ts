import { Component } from '@angular/core';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-tasks-landing',
  templateUrl: './landing.component.html',
})
export class TasksLandingComponent {
    readonly routingNodes = [
        {
            routerLink: '/' + RoutingNodes.Tasks + '/' + RoutingNodes.Overview,
            title: 'app.navigation.tasks.tasks_overview_title',
            icon: 'table_chart',
            bodyMessage: 'app.navigation.tasks.tasks_overview_description',
        },
        {
            routerLink: '/' + RoutingNodes.Tasks + '/' + RoutingNodes.CreateTaskLink,
            title: 'app.navigation.tasks.create_task_link_title',            
            icon: 'link',
            bodyMessage: 'app.navigation.tasks.create_task_link_description',
        },
    ];

}