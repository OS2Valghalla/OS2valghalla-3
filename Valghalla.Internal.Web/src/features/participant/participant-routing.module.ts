import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ParticipantLandingComponent } from './components/landing/landing.component';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { ParticipantItemComponent } from './components/participant-item/participant-item.component';
import { AuthGuard } from 'src/app/auth/auth.guard';
import { ParticipantProfileComponent } from './components/participant-profile/participant-profile.component';

const routes: Routes = [
  { path: '', component: ParticipantLandingComponent },
  {
    path: RoutingNodes.Link_Create,
    title: 'participant.participant_item.page_title.create',
    data: { breadcrumb: 'participant.participant_item.page_title.create' },
    component: ParticipantItemComponent,
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.Link_Create + '/:workLocationId' + '/:taskId',
    title: 'participant.participant_item.page_title.create',
    data: { breadcrumb: 'participant.participant_item.page_title.create' },
    component: ParticipantItemComponent,
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.Link_Edit + '/:id',
    title: 'participant.participant_item.page_title.edit',
    data: { breadcrumb: 'participant.participant_item.page_title.edit' },
    component: ParticipantItemComponent,
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.Profile + '/:id',
    title: 'participant.participant_profile.title',
    data: { breadcrumb: 'participant.participant_profile.title' },
    component: ParticipantProfileComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ParticipantRoutingModule {}
