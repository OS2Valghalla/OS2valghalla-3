import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TaskDistributionLandingComponent } from './components/landing.component';
import { TaskDistributionDetailsComponent } from './components/task-details/task-details.component';
import { AuthGuard } from 'src/app/auth/auth.guard';

const routes: Routes = [
  {
    path: '',
    title: 'task_distribution.landing.page_title',
    component: TaskDistributionLandingComponent,
    canActivate: [AuthGuard]
  },
  {
    path: RoutingNodes.TaskDetails + '/:id',
    title: 'tasks.task_details.page_title',
    component: TaskDistributionDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TaskDistributionRoutingModule {}
