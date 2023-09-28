import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { AuthGuard } from 'src/app/auth/auth.guard';
import { CommunicationLandingComponent } from './landing.component';

const routes: Routes = [
  {
    path: '',
    title: 'communication.page_title',
    data: { breadcrumb: 'communication.page_title' },
    component: CommunicationLandingComponent,
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.CommunicationTemplates,
    loadChildren: () =>
      import('./communication-template/communication-template-routing.module').then(
        (m) => m.CommunicationTemplateRoutingModule,
      ),
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.CommunicationLogs,
    loadChildren: () =>
      import('./communication-log/communication-log-routing.module').then((m) => m.CommunicationLogRoutingModule),
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.SendMessage,
    loadChildren: () =>
      import('./send-message/send-message-routing.module').then(
        (m) => m.CommunicationSendMessageRoutingModule,
      ),
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CommunicationRoutingModule {}
