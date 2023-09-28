import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../app/auth/auth.guard';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TasksOverviewComponent } from '../tasks/components/tasks-overview/tasks-overview.component';

const routes: Routes = [
  { path: '', component: TasksOverviewComponent },
  {
    path: RoutingNodes.TasksOnWorkLocation + '/:' + RoutingNodes.WorkLocationId,
    loadChildren: () => import('../tasks/components/work-location-tasks-overview/work-location-tasks-overview-routing.module').then((m) => m.WorkLocationTasksRoutingModule),
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class StartRoutingModule { }
