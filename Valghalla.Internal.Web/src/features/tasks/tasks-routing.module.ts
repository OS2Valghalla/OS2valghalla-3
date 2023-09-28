import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TasksLandingComponent } from './components/landing/landing.component';
import { TasksOverviewComponent } from './components/tasks-overview/tasks-overview.component';
import { CreateTaskLinkComponent } from './components/create-task-link/create-task-link.component';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

const routes: Routes = [
    {
        path: '',
        title: 'app.navigation.tasks.title',
        component: TasksLandingComponent,
    },
    {
        path: RoutingNodes.Overview,
        data: { breadcrumb: 'app.navigation.tasks.tasks_overview_title' },
        title: 'app.navigation.tasks.tasks_overview_title',
        component: TasksOverviewComponent,
    },
    {
        path: RoutingNodes.CreateTaskLink,
        data: { breadcrumb: 'app.navigation.tasks.create_task_link_title' },
        title: 'app.navigation.tasks.create_task_link_title',
        component: CreateTaskLinkComponent,
    },  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TasksRoutingModule {}