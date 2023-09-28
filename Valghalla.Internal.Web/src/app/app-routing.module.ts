import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

const routes: Routes = [
  {
    path: '_',
    title: 'unprotected.page_title',
    loadChildren: () => import('../features/unprotected/unprotected.module').then((m) => m.UnprotectedModule),
  },
  {
    path: '',
    title: 'start.page_title',
    data: { breadcrumb: 'start.page_title' },
    loadChildren: () => import('../features/start/start.module').then((m) => m.StartModule),
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.Tasks,
    title: 'tasks.page_title',
    data: { breadcrumb: 'tasks.page_title' },
    loadChildren: () => import('../features/tasks/tasks.module').then((m) => m.TasksModule),
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.List,
    title: 'administration.page_title',
    data: { breadcrumb: 'list.page_title' },
    loadChildren: () => import('../features/list/list.module').then((m) => m.ListModule),
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.Administration,
    title: 'administration.page_title',
    data: { breadcrumb: 'administration.page_title' },
    loadChildren: () => import('../features/administration/administration.module').then((m) => m.AdministrationModule),
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.Participant,
    title: 'participant.page_title',
    data: { breadcrumb: 'participant.page_title' },
    loadChildren: () => import('../features/participant/participant.module').then((m) => m.PartiticipantModule),
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.Communication,
    title: 'app.navigation.communication.title',
    data: { breadcrumb: 'app.navigation.communication.title' },
    loadChildren: () => import('../features/communication/communication.module').then((m) => m.CommunicationModule),
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
