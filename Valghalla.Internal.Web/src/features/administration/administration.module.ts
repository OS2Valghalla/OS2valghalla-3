import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslocoModule } from '@ngneat/transloco';
import { MaterialModule } from 'src/shared/material.module';
import { SharedModule } from 'src/shared/shared.module';
import { AdministrationRoutingModule } from './administration-routing.module';
import { ElectionItemComponent } from './election/components/election-item/election-item.component';
import { DuplicateElectionComponent } from './election/components/duplicate-election/duplicate-election.component';
import { ElectionComponent } from './election/election.component';
import { CommunicationTemplateSelectionComponent } from './election/components/communication-template-selection/communication-template-selection.component';
import { EditElectionCommunicationConfigurationComponent } from './election/components/edit-election-communication-configuration/edit-election-communication-configuration.component';
import { ElectionTypeComponent } from './election-type/election-type.component';
import { ElectionTypeItemComponent } from './election-type/components/election-type-item/election-type-item.component';
import { LandingComponent } from './landing/landing.component';
import { UserEditDialogComponent } from './user/components/user-edit-dialog/user-edit-dialog.component';
import { UserInvitationDialogComponent } from './user/components/user-invitation/user-invitation-dialog.component';
import { UserComponent } from './user/user.component';
import { SpecialDietComponent } from './specialdiet/specialdiet.component';
import { SpecialDietItemComponent } from './specialdiet/components/specialdiet-item.component';
import { TeamsComponent } from './teams/teams.component';
import { TeamItemComponent } from './teams/components/team-item/team-item.component';
import { AreaComponent } from './area/area.component';
import { AreaItemComponent } from './area/components/area-item/area-item.component';
import { WebLandingComponent } from './web/landing.component';
import { WebDisclosureStatementComponent } from './web/components/disclosure-statement-page.component';
import { WebDeclarationOfConsentComponent } from './web/components/declaration-of-consent-page.component';
import { WebFrontPageComponent } from './web/components/front-page.component';
import { WebFAQPageComponent } from './web/components/faq-page.component';
import { WebContactInformationComponent } from './web/components/contact-information.component';
import { TaskTypeItemComponent } from './task-type/components/task-type-item/task-type-item.component';
import { TaskTypeComponent } from './task-type/task-type.component';
import { WorkLocationComponent } from './work-location/work-location.component';
import { WorkLocationItemComponent } from './work-location/components/work-location-item/work-location-item.component';
import { AuditLogLandingComponent } from './audit-log/audit-log.component';
import { CopyInviteLinkDialogComponent } from './teams/components/copy-invite-link-dialog/copy-invite-link-dialog.component';

@NgModule({
  declarations: [
    LandingComponent,
    ElectionComponent,
    ElectionTypeComponent,
    ElectionTypeItemComponent,
    ElectionItemComponent,
    DuplicateElectionComponent,
    SpecialDietComponent,
    SpecialDietItemComponent,
    UserComponent,
    UserInvitationDialogComponent,
    UserEditDialogComponent,
    CopyInviteLinkDialogComponent,
    TeamsComponent,
    TeamItemComponent,
    AreaComponent,
    AreaItemComponent,
    WebLandingComponent,
    WebDisclosureStatementComponent,
    WebDeclarationOfConsentComponent,
    WebContactInformationComponent,
    WebFrontPageComponent,
    WebFAQPageComponent,
    TaskTypeComponent,
    TaskTypeItemComponent,
    WorkLocationComponent,
    WorkLocationItemComponent,
    CommunicationTemplateSelectionComponent,
    EditElectionCommunicationConfigurationComponent,
    AuditLogLandingComponent,
  ],
  imports: [CommonModule, MaterialModule, AdministrationRoutingModule, SharedModule, TranslocoModule],
})
export class AdministrationModule { }
