import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { ReactiveFormsModule } from '@angular/forms';
import { MyWorkLocationLandingComponent } from './components/landing.component';
import { MyWorkLocationRoutingModule } from './my-work-location-routing.module';
import { DkfdsModule } from 'src/shared/dkfds/dkfds.module';

@NgModule({
  declarations: [MyWorkLocationLandingComponent],
  imports: [CommonModule, MyWorkLocationRoutingModule, SharedModule, TranslocoModule, DkfdsModule, ReactiveFormsModule],
})
export class MyWorkLocationModule {}
