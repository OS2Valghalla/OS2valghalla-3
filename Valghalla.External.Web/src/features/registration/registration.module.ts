import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { DkfdsModule } from 'src/shared/dkfds/dkfds.module';
import { ReactiveFormsModule } from '@angular/forms';
import { RegistrationRoutingModule } from './registration-routing.module';
import { TeamRegistrationLandingComponent } from './components/team-registration-landing/team-registration-landing.component';
import { TaskRegistrationLandingComponent } from './components/task-registration-landing/task-registration-landing.component';
import { ParticipantRegisterComponent } from './components/participant-register/participant-register.component';
import { DisclosureStatementConfirmatorComponent } from './components/disclosure-statement-confirmator/disclosure-statement-confirmator.component';
import { RegistrationStepIndicatorComponent } from './components/registration-step-indicator/registration-step-indicator.component';
import { MyProfileRegistrationConfirmatorComponent } from './components/my-profile-registration-confirmator/my-profile-registration-confirmator.component';

@NgModule({
  declarations: [
    TeamRegistrationLandingComponent,
    TaskRegistrationLandingComponent,
    DisclosureStatementConfirmatorComponent,
    ParticipantRegisterComponent,
    RegistrationStepIndicatorComponent,
    MyProfileRegistrationConfirmatorComponent,
  ],
  imports: [CommonModule, ReactiveFormsModule, DkfdsModule, SharedModule, TranslocoModule, RegistrationRoutingModule],
  exports: [RegistrationStepIndicatorComponent],
})
export class RegistrationModule {}
