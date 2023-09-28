import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized.component';
import { UnprotectedRoutingModule } from './unprotected-routing.module';
import { LogoutComponent } from './components/logout/logout.component';
import { MaterialModule } from 'src/shared/material.module';
import { NotFoundComponent } from './components/notfound/notfound.component';

@NgModule({
  declarations: [
    UnauthorizedComponent,
    LogoutComponent,
    NotFoundComponent,
  ],
  imports: [
    CommonModule,
    UnprotectedRoutingModule,
    SharedModule,
    TranslocoModule,
    MaterialModule,
  ],
})
export class UnprotectedModule {}
