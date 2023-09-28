import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { ReactiveFormsModule } from '@angular/forms';
import { MyTeamLandingComponent } from './components/landing.component';
import { MyTeamRoutingModule } from './my-team-routing.module';
import { DkfdsModule } from 'src/shared/dkfds/dkfds.module';

@NgModule({
  declarations: [MyTeamLandingComponent],
  imports: [CommonModule, MyTeamRoutingModule, SharedModule, TranslocoModule, DkfdsModule, ReactiveFormsModule],
})
export class MyTeamModule {}
