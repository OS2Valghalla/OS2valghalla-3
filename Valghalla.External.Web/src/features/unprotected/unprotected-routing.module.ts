import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { UnprotectedLandingComponent } from './landing/landing.component';
import { PlaygroundComponent } from './playground/playground.component';
import { LogoutComponent } from './logout/logout.component';
import { AnnonymousGuard } from 'src/app/auth/annonymous.guard';

const routes: Routes = [
  {
    path: '',
    title: 'unprotected.landing.page_title',
    component: UnprotectedLandingComponent,
  },
  {
    path: 'playground',
    title: 'Playground',
    component: PlaygroundComponent,
  },
  {
    path: RoutingNodes.Logout,
    title: 'Logout',
    component: LogoutComponent,
    canActivate: [AnnonymousGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UnprotectedRoutingModule {}
