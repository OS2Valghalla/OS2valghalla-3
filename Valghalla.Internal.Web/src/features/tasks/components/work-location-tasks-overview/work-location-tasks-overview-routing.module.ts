import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WorkLocationTasksOverviewComponent } from '../work-location-tasks-overview/work-location-tasks-overview.component';
import { WorkLocationTasksDistributionComponent } from '../work-location-tasks-distribution/work-location-tasks-distribution.component';
import { ReplyForParticipantComponent } from '../reply-for-participant/reply-for-participant.component';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

const routes: Routes = [
  {
    path: '',
    title: 'app.navigation.tasks.work_location_tasks_title',
    data: { breadcrumb: ' ' },
    component: WorkLocationTasksOverviewComponent,
  },
  {
    path: RoutingNodes.Link_Distribute,
    title: 'app.navigation.tasks.work_location_tasks_distribution_title',
    data: { breadcrumb: 'app.navigation.tasks.work_location_tasks_distribution_title' },
    component: WorkLocationTasksDistributionComponent,
  },
  {
    path: RoutingNodes.ReplyForParticipant + '/:' + RoutingNodes.TaskId,
    title: 'app.navigation.tasks.reply_for_participant',
    data: { breadcrumb: 'app.navigation.tasks.reply_for_participant' },
    component: ReplyForParticipantComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorkLocationTasksRoutingModule {}
