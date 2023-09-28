import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyProfileLandingComponent } from './components/landing/landing.component';

const routes: Routes = [
  {
    path: '',
    title: 'my_profile.landing.page_title',
    component: MyProfileLandingComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MyProfileRoutingModule {}
