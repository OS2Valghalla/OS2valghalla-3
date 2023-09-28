import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyTeamLandingComponent } from './components/landing.component';

const routes: Routes = [
  {
    path: '',
    title: 'my_team.landing.page_title',
    component: MyTeamLandingComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MyTeamRoutingModule {}
