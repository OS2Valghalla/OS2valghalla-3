import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingComponent } from './landing/landing.component';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

const routes: Routes = [
  {
    path: '',
    title: 'administration.page_title',
    component: LandingComponent,
  },
  {
    path: RoutingNodes.ElectionType,
    loadChildren: () =>
      import('../administration/election-type/election-type.routing.module').then((m) => m.ElectionTypeRoutingModule),
  },
  {
    path: RoutingNodes.Election,
    loadChildren: () =>
      import('../administration/election/election.routing.module').then((m) => m.ElectionRoutingModule),
  },
  {
    path: RoutingNodes.Teams,
    loadChildren: () => import('../administration/teams/teams.routing.module').then((m) => m.TeamsRoutingModule),
  },
  {
    path: RoutingNodes.SpecialDiet,
    loadChildren: () =>
      import('../administration/specialdiet/specialdiet.routing.module').then((m) => m.SpecialDietRoutingModule),
  },
  {
    path: RoutingNodes.Area,
    loadChildren: () => import('../administration/area/area.routing.module').then((m) => m.AreaRoutingModule),
  },
  {
    path: RoutingNodes.Web,
    loadChildren: () => import('../administration/web/web.routing.module').then((m) => m.WebRoutingModule),
  },
  {
    path: RoutingNodes.WorkLocation,
    loadChildren: () =>
      import('../administration/work-location/work-location.routing.module').then((m) => m.WorkLocationRoutingModule),
  },
  {
    path: RoutingNodes.TaskType,
    loadChildren: () =>
      import('../administration/task-type/task-type.routing.module').then((m) => m.TaskTypeRoutingModule),
  },
  {
    path: RoutingNodes.AuditLog,
    loadChildren: () =>
      import('../administration/audit-log/audit-log.routing.module').then((m) => m.AuditLogRoutingModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdministrationRoutingModule {}
