import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { ReactiveFormsModule } from '@angular/forms';
import { TaskDistributionLandingComponent } from './components/landing.component';
import { TaskDistributionDetailsComponent } from './components/task-details/task-details.component';
import { TaskDistributionRoutingModule } from './task-distribution-routing.module';
import { DkfdsModule } from 'src/shared/dkfds/dkfds.module';
import { TasksModule } from '../tasks/tasks.module';

@NgModule({
  declarations: [TaskDistributionLandingComponent, TaskDistributionDetailsComponent],
  imports: [CommonModule, TaskDistributionRoutingModule, SharedModule, TranslocoModule, DkfdsModule, ReactiveFormsModule, TasksModule]
})
export class TaskDistributionModule {}
