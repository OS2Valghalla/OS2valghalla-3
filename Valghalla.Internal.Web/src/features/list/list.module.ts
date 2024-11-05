import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListRoutingModule } from './list-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { MaterialModule } from '../../shared/material.module';
import { TranslocoModule } from '@ngneat/transloco';
import { ListLandingComponent } from './components/landing/landing.component';
import { ParticipantListComponent } from './components/participant-list/participant-list.component';
import { ElectionSystemList } from './components/election-system-list/election-system-list.component';

@NgModule({
    declarations: [
        ListLandingComponent,
        ParticipantListComponent,
        ElectionSystemList
    ],
    imports: [CommonModule, ListRoutingModule, SharedModule, MaterialModule, TranslocoModule],
  })
  export class ListModule {}