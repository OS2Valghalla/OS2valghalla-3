import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { ReactiveFormsModule } from '@angular/forms';
import { MyTasksLandingComponent } from './components/landing.component';
import { MyTasksRoutingModule } from './my-tasks-routing.module';
import { DkfdsModule } from 'src/shared/dkfds/dkfds.module';
import { TasksModule } from '../tasks/tasks.module';

@NgModule({
  declarations: [MyTasksLandingComponent],
  imports: [CommonModule, MyTasksRoutingModule, SharedModule, TranslocoModule, DkfdsModule, ReactiveFormsModule, TasksModule],
})
export class MyTasksModule {}
