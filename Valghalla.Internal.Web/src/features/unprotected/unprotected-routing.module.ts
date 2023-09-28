import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnonymousGuard } from './anonymous.guard';
import { LogoutComponent } from './components/logout/logout.component';
import { NotFoundComponent } from './components/notfound/notfound.component';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized.component';

const routes: Routes = [
  {
    path: 'unauthorized',
    title: 'unprotected.unauthorized.page_title',
    component: UnauthorizedComponent,
  },
  {
    path: 'logout',
    title: 'unprotected.logout.page_title',
    component: LogoutComponent,
    canActivate: [AnonymousGuard],
  },
  {
    path: 'not-found',
    title: 'unprotected.notfound.page_title',
    component: NotFoundComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UnprotectedRoutingModule {}
