import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslocoModule } from '@ngneat/transloco';
import { MaterialModule } from 'src/shared/material.module';
import { SharedModule } from 'src/shared/shared.module';
import { TasksRoutingModule } from './tasks-routing.module';
import { TasksLandingComponent } from './components/landing/landing.component';
import { TasksOverviewComponent } from './components/tasks-overview/tasks-overview.component';
import { CreateTaskLinkComponent } from './components/create-task-link/create-task-link.component';
import { WorkLocationTasksOverviewComponent } from './components/work-location-tasks-overview/work-location-tasks-overview.component';
import { WorkLocationTasksDistributionComponent } from './components/work-location-tasks-distribution/work-location-tasks-distribution.component';
import { ReplyForParticipantComponent } from './components/reply-for-participant/reply-for-participant.component';

@NgModule({
    declarations: [
        TasksLandingComponent,
        TasksOverviewComponent,
        CreateTaskLinkComponent,
        WorkLocationTasksOverviewComponent,
        WorkLocationTasksDistributionComponent,
        ReplyForParticipantComponent,
    ],
    imports: [CommonModule, MaterialModule, TasksRoutingModule, SharedModule, TranslocoModule],
  })
  export class TasksModule {}