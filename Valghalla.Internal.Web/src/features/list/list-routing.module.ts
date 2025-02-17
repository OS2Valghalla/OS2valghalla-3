import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { AuthGuard } from 'src/app/auth/auth.guard';
import { ListLandingComponent } from './components/landing/landing.component';
import { ParticipantListComponent } from './components/participant-list/participant-list.component';
import { ElectionSystemList } from './components/election-system-list/election-system-list.component';

const routes: Routes = [
  {
    path: '',
    title: 'list.page_title',
    data: { breadcrumb: 'list.page_title' },
    component: ListLandingComponent,
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.ParticipantList,
    title: 'list.participant_list.page_title',
    data: { breadcrumb: 'list.participant_list.page_title' },
    component: ParticipantListComponent,
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.ElectionSystemList,
    title: 'list.election_system_list.page_title',
    data: { breadcrumb: 'list.election_system_list.page_title' },
    component: ElectionSystemList,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ListRoutingModule {}
