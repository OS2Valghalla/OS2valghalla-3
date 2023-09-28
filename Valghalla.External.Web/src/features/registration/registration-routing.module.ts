import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TeamRegistrationLandingComponent } from './components/team-registration-landing/team-registration-landing.component';
import { TaskRegistrationLandingComponent } from './components/task-registration-landing/task-registration-landing.component';
import { TaskRegistrationGuard } from './task-registration.guard';
import { TaskCancellationGuard } from './task-cancellation.guard';
import { TeamRegistrationGuard } from './team-registration.guard';
import { TaskHttpService } from '../tasks/services/task-http.service';
import { RegistrationHttpService } from './services/registration-http.service';

const routes: Routes = [
  {
    path: RoutingNodes.TeamRegistration + '/:id',
    title: 'registration.landing.page_title',
    component: TeamRegistrationLandingComponent,
    canActivate: [TeamRegistrationGuard],
    providers: [RegistrationHttpService, TeamRegistrationGuard]
  },
  {
    path: RoutingNodes.TaskRegistration + '/:id',
    title: 'registration.landing.page_title',
    component: TaskRegistrationLandingComponent,
    canActivate: [TaskRegistrationGuard],
    providers: [TaskHttpService, TaskRegistrationGuard],
  },
  {
    path: RoutingNodes.TaskRegistration + '/' + RoutingNodes.Cancellation + '/:id',
    title: 'registration.landing.page_title',
    component: TaskRegistrationLandingComponent,
    canActivate: [TaskCancellationGuard],
    providers: [TaskHttpService, TaskCancellationGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RegistrationRoutingModule {}
