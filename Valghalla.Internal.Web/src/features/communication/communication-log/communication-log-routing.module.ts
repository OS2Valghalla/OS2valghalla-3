import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { AuthGuard } from 'src/app/auth/auth.guard';
import { CommunicationLogLandingComponent } from './components/landing/landing.component';
import { CommunicationLogDetailsComponent } from './components/communication-log-details/communication-log-details.component';

const routes: Routes = [
  {
    path: '',
    title: 'communication.communication_log.page_title',
    data: { breadcrumb: 'communication.communication_log.page_title' },
    component: CommunicationLogLandingComponent,
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.Details + '/:id',
    title: 'communication.communication_log_details.page_title',
    data: { breadcrumb: 'communication.communication_log_details.page_title' },
    component: CommunicationLogDetailsComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CommunicationLogRoutingModule {}
