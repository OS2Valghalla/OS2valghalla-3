import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { TasksLandingComponent } from './components/landing/landing.component';
import { TasksRoutingModule } from './tasks-routing.module';
import { DkfdsModule } from 'src/shared/dkfds/dkfds.module';
import { TaskPreviewComponent } from './components/task-preview/task-preview.component';
import { TaskInvitationComponent } from './components/task-invitation/task-invitation.component';
import { TaskRejectionConfirmationComponent } from './components/task-rejection-confirmation/task-rejection-confirmation.component';
import { TaskAcceptanceConflictComponent } from './components/task-acceptance-conflict/task-acceptance-conflict.component';
import { TaskAcceptanceValidationFailureComponent } from './components/task-acceptance-validation-failure/task-acceptance-validation-failure.component';
import { TaskAcceptanceCprInvalidComponent } from './components/task-acceptance-cpr-invalid/task-acceptance-cpr-invalid.component';
import { TaskAcceptanceConfirmationComponent } from './components/task-acceptance-confirmation/task-acceptance-confirmation.component';
import { TaskDetailsComponent } from './components/task-details/task-details.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RegistrationModule } from '../registration/registration.module';

@NgModule({
  declarations: [
    TasksLandingComponent,
    TaskPreviewComponent,
    TaskDetailsComponent,
    TaskInvitationComponent,
    TaskRejectionConfirmationComponent,
    TaskAcceptanceConflictComponent,
    TaskAcceptanceValidationFailureComponent,
    TaskAcceptanceCprInvalidComponent,
    TaskAcceptanceConfirmationComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TasksRoutingModule,
    SharedModule,
    TranslocoModule,
    DkfdsModule,
    RegistrationModule,
  ],
  exports: [TaskPreviewComponent],
})
export class TasksModule {}
