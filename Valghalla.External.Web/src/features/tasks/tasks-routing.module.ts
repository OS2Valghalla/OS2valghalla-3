import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TasksLandingComponent } from './components/landing/landing.component';
import { TaskInvitationComponent } from './components/task-invitation/task-invitation.component';
import { TaskRejectionConfirmationComponent } from './components/task-rejection-confirmation/task-rejection-confirmation.component';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TaskAcceptanceConflictComponent } from './components/task-acceptance-conflict/task-acceptance-conflict.component';
import { TaskAcceptanceValidationFailureComponent } from './components/task-acceptance-validation-failure/task-acceptance-validation-failure.component';
import { TaskAcceptanceCprInvalidComponent } from './components/task-acceptance-cpr-invalid/task-acceptance-cpr-invalid.component';
import { TaskAcceptanceConfirmationComponent } from './components/task-acceptance-confirmation/task-acceptance-confirmation.component';
import { TaskDetailsComponent } from './components/task-details/task-details.component';
import { AuthGuard } from 'src/app/auth/auth.guard';

const routes: Routes = [
  {
    path: '',
    title: 'tasks.landing.page_title',
    component: TasksLandingComponent,
    canActivate: [AuthGuard]
  },
  {
    path: RoutingNodes.TaskDetails + '/:id',
    title: 'tasks.task_details.page_title',
    component: TaskDetailsComponent,
  },
  {
    path: RoutingNodes.TaskInvitation + '/:id' + '/:code',
    title: 'tasks.task_invitation.page_title',
    component: TaskInvitationComponent,
  },
  {
    path: RoutingNodes.TaskRejectionConfirmation + '/:id',
    title: 'tasks.task_rejection_confirmation.page_title',
    component: TaskRejectionConfirmationComponent,
  },
  {
    path: RoutingNodes.TaskAcceptanceCprInvalid,
    title: 'tasks.task_acceptance_cpr_invalid.page_title',
    component: TaskAcceptanceCprInvalidComponent,
  },
  {
    path: RoutingNodes.TaskAcceptanceConflict,
    title: 'tasks.task_acceptance_conflict.page_title',
    component: TaskAcceptanceConflictComponent,
  },
  {
    path: RoutingNodes.TaskAcceptanceValidationFailure + '/:values',
    title: 'tasks.task_acceptance_validation_failure.page_title',
    component: TaskAcceptanceValidationFailureComponent,
  },
  {
    path: RoutingNodes.TaskAcceptanceConfirmation + '/:id',
    title: 'tasks.task_acceptance_confirmation.page_title',
    component: TaskAcceptanceConfirmationComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TasksRoutingModule {}
