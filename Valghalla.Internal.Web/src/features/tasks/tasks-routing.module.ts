import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TasksLandingComponent } from './components/landing/landing.component';
import { TasksOverviewComponent } from './components/tasks-overview/tasks-overview.component';
import { CreateTaskLinkComponent } from './components/create-task-link/create-task-link.component';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { RejectedTasksOverviewComponent } from './components/rejected-tasks-overview/rejected-tasks-overview.component';

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
    {
        path: RoutingNodes.RejectedTasksOverview,
        data: { breadcrumb: 'app.navigation.tasks.rejected_tasks_overview_link_title' },
        title: 'app.navigation.tasks.rejected_tasks_overview_link_title',
        component: RejectedTasksOverviewComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TasksRoutingModule { }