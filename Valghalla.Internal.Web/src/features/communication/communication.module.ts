import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommunicationRoutingModule } from './communication-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { MaterialModule } from '../../shared/material.module';
import { TranslocoModule } from '@ngneat/transloco';
import { CommunicationTemplateItemComponent } from './communication-template/components/communication-template-item/communication-template-item.component';
import { CommunicationTemplatesComponent } from './communication-template/components/communication-templates/communication-templates.component';
import { CommunicationLogLandingComponent } from './communication-log/components/landing/landing.component';
import { CommunicationLandingComponent } from './landing.component';
import { CommunicationLogDetailsComponent } from './communication-log/components/communication-log-details/communication-log-details.component';
import { CommunicationSendMessageComponent } from './send-message/components/send-message.component';

@NgModule({
  declarations: [
    CommunicationLandingComponent,
    CommunicationTemplatesComponent,
    CommunicationTemplateItemComponent,
    CommunicationLogLandingComponent,
    CommunicationLogDetailsComponent,
    CommunicationSendMessageComponent
  ],
  imports: [CommonModule, CommunicationRoutingModule, SharedModule, MaterialModule, TranslocoModule],
})
export class CommunicationModule {}
