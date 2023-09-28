import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { ContactUsComponent } from '../features/contact-us/contact-us.component';
import { PrivacyPolciyComponent } from '../features/privacy-policy/privacy-policy.component';
import { FilteredTasksComponent } from '../features/tasks/components/filtered-tasks/filtered-tasks.component';
import { TeamInvitationComponent } from '../features/unprotected/team-invitation/team-invitation.component';
import { ElectionGuard } from './election.guard';
import { FaqPageGuard } from './faq-page.guard';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('../features/unprotected/unprotected.module').then((m) => m.UnprotectedModule),
  },
  {
    path: RoutingNodes.Tasks,
    loadChildren: () => import('../features/tasks/tasks.module').then((m) => m.TasksModule),
    canActivate: [ElectionGuard],
  },
  {
    path: RoutingNodes.TaskDistribution,
    loadChildren: () =>
      import('../features/task-distribution/task-distribution.module').then((m) => m.TaskDistributionModule),
    canActivate: [ElectionGuard, AuthGuard],
  },
  {
    path: RoutingNodes.MyWorkLocation,
    loadChildren: () =>
      import('../features/my-work-location/my-work-location.module').then((m) => m.MyWorkLocationModule),
    canActivate: [ElectionGuard, AuthGuard],
  },
  {
    path: RoutingNodes.MyTeam,
    loadChildren: () => import('../features/my-team/my-team.module').then((m) => m.MyTeamModule),
    canActivate: [ElectionGuard, AuthGuard],
  },
  {
    path: RoutingNodes.MyTasks,
    loadChildren: () => import('../features/my-tasks/my-tasks.module').then((m) => m.MyTasksModule),
    canActivate: [ElectionGuard, AuthGuard],
  },
  {
    path: RoutingNodes.MyProfile,
    loadChildren: () => import('../features/my-profile/my-profile.module').then((m) => m.MyProfileModule),
    canActivate: [ElectionGuard, AuthGuard],
  },
  {
    path: RoutingNodes.Faq,
    loadChildren: () => import('../features/faq/faq.module').then((m) => m.FaqModule),
    canActivate: [FaqPageGuard]
  },
  {
    path: RoutingNodes.ContactUs,
    title: 'app.footer.contact_us',
    component: ContactUsComponent,
  },
  {
    path: RoutingNodes.PrivacyPolicy,
    title: 'app.footer.privacy_policy',
    component: PrivacyPolciyComponent,    
  },
  {
    path: RoutingNodes.TaskOverview,
    title: 'tasks.task_overview.page_title',
    component: FilteredTasksComponent,
    canActivate: [ElectionGuard],
  },
  {
    path: RoutingNodes.TeamLink,
    title: 'my_team.labels.team_invitation',
    component: TeamInvitationComponent,
    canActivate: [ElectionGuard],
  },
  {
    path: RoutingNodes.Registration,
    loadChildren: () => import('../features/registration/registration.module').then((m) => m.RegistrationModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
