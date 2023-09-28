import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ParticipantLandingComponent } from './components/landing/landing.component';
import { ParticipantRoutingModule } from './participant-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { MaterialModule } from '../../shared/material.module';
import { TranslocoModule } from '@ngneat/transloco';
import { ParticipantItemComponent } from './components/participant-item/participant-item.component';
import { ParticipantInformationComponent } from './components/participant-profile/participant-information/participant-information.component';
import { ParticipantValidationInformationComponent } from './components/participant-profile/participant-validation-information/participant-validation-information.component';
import { TeamResponsibleRightsComponent } from './components/participant-profile/team-responsible-rights/team-responsible-rights.component';
import { ParticipantEventLogItemComponent } from './components/participant-profile/participant-event-log-listing/participant-event-log-item.component';
import { ParticipantEventLogListingComponent } from './components/participant-profile/participant-event-log-listing/participant-event-log-listing.component';
import { ParticipantProfileComponent } from './components/participant-profile/participant-profile.component';
import { WorkLocationResponsibleRightsComponent } from './components/participant-profile/work-location-responsible-rights/work-location-responsible-rights.component';
import { ParticipantTaskStatusListingComponent } from './components/participant-profile/participant-task-status-listing/participant-task-status-listing.component';
import { ParticipantCommunicationLogListingComponent } from './components/participant-profile/participant-communication-log-listing/participant-communication-log-listing.component';

@NgModule({
  declarations: [
    ParticipantLandingComponent,
    ParticipantItemComponent,
    ParticipantInformationComponent,
    ParticipantValidationInformationComponent,
    TeamResponsibleRightsComponent,
    ParticipantEventLogItemComponent,
    ParticipantEventLogListingComponent,
    ParticipantProfileComponent,
    WorkLocationResponsibleRightsComponent,
    ParticipantTaskStatusListingComponent,
    ParticipantCommunicationLogListingComponent,
  ],
  imports: [CommonModule, ParticipantRoutingModule, SharedModule, MaterialModule, TranslocoModule],
})
export class PartiticipantModule {}
