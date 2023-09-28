import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { UnprotectedRoutingModule } from './unprotected-routing.module';
import { UnprotectedLandingComponent } from './landing/landing.component';
import { PlaygroundComponent } from './playground/playground.component';
import { DkfdsModule } from 'src/shared/dkfds/dkfds.module';
import { ReactiveFormsModule } from '@angular/forms';
import { LogoutComponent } from './logout/logout.component';

@NgModule({
  declarations: [UnprotectedLandingComponent, PlaygroundComponent, LogoutComponent],
  imports: [CommonModule, ReactiveFormsModule, DkfdsModule, SharedModule, TranslocoModule, UnprotectedRoutingModule],
})
export class UnprotectedModule {}
