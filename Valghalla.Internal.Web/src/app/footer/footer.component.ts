import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { User } from 'src/app/models/user';
import { NotificationService } from 'src/shared/services/notification.service';
import { SubSink } from 'subsink';
import { AuthService } from '../auth/auth.service';
import { GlobalStateService } from '../global-state.service';
import { ChangeElectionDialogComponent } from './change-election-dialog/change-election-dialog.component';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
})
export class FooterComponent implements OnInit, OnDestroy {
  private subs = new SubSink();

  @Input() isDesktopView: boolean;

  currentUser: User;

  currentMunicipality = '<Municipality name>';

  externalWebUrl = '';

  constructor(
    private translocoService: TranslocoService,
    private notificationService: NotificationService,
    private globalStateService: GlobalStateService,
    private dialog: MatDialog,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    this.subs.sink = this.globalStateService.user$.subscribe((user) => {
      this.currentUser = user;
    });
    this.subs.sink = this.globalStateService.municipalityName$.subscribe((municipalityName) => {
      this.currentMunicipality = municipalityName;
    });
    this.subs.sink = this.globalStateService.externalWebUrl$.subscribe((externalWebUrl) => {
      this.externalWebUrl = externalWebUrl;
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onChangeLang() {
    const activeLang = this.translocoService.getActiveLang();
    if (activeLang === 'da') this.translocoService.setActiveLang('en');
    else this.translocoService.setActiveLang('da');

    this.notificationService.showSuccess('Successfully changed language');
  }

  onChangeActiveElection() {
    this.dialog.open(ChangeElectionDialogComponent);
  }

  onLogout() {
    this.authService.logout();
  }
}
