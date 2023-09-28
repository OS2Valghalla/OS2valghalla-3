import { Component, OnDestroy, OnInit, AfterViewInit } from '@angular/core';
import { SubSink } from 'subsink';
import { User } from '../models/user';
import { GlobalStateService } from '../global-state.service';
import { Role } from '../../shared/constants/role';
import { Navigation } from 'dkfds';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { AuthService } from '../auth/auth.service';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';
import { ElectionCommitteeContactInformationPage } from 'src/shared/models/web-page';
import { AppHttpService } from '../app-http.service';
import { Router, RoutesRecognized } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: 'header.component.html',
  styleUrls: ['header.component.scss'],
})
export class HeaderComponent implements OnDestroy, OnInit, AfterViewInit {
  private readonly subs = new SubSink();
  private readonly dkfdsNav = new Navigation();

  readonly municipalityLogoUrl: string = getBaseApiUrl() + 'app/logo';

  readonly links = {
    faq: '/' + RoutingNodes.Faq,
    myProfile: '/' + RoutingNodes.MyProfile,
    myTasks: '/' + RoutingNodes.MyTasks,
    myTeam: '/' + RoutingNodes.MyTeam,
    myWorkLocation: '/' + RoutingNodes.MyWorkLocation,
    taskDistribution: '/' + RoutingNodes.TaskDistribution,
    tasks: '/' + RoutingNodes.Tasks,
  };

  get signedIn() {
    return this.authService.isSessionExist();
  }

  get participantPermission() {
    return this.currentUser?.roleIds.includes(Role.participant);
  }

  get teamResponsiblePermission() {
    return this.currentUser?.roleIds.includes(Role.teamResponsible);
  }

  get worklocationResponsiblePermission() {
    return this.currentUser?.roleIds.includes(Role.workLocationResponsible);
  }

  faqPageActivated: boolean;

  currentUser?: User;

  contactUsUrl: string;

  webPage?: ElectionCommitteeContactInformationPage;

  userTeamNames?: string;

  focused: boolean = false;

  constructor(
    private readonly router: Router,
    private readonly globalStateService: GlobalStateService,
    private readonly authService: AuthService,
    private readonly appHttpService: AppHttpService,
  ) {}

  ngOnInit() {
    this.contactUsUrl = RoutingNodes.ContactUs;

    this.getGlobalData();

    this.subs.sink = this.router.events.subscribe((event) => {
      if (event instanceof RoutesRecognized) {
        this.focused = event.url.includes(RoutingNodes.Registration);
      }
    });

    this.subs.sink = this.authService.authorized$.subscribe((authorized) => {
      if (authorized) {
        this.globalStateService.userTeamFetching.next();
      }
    });
  }

  ngAfterViewInit(): void {
    this.dkfdsNav.init();
  }

  ngOnDestroy() {
    this.dkfdsNav.teardown();
    this.subs.unsubscribe();
  }

  getGlobalData() {
    this.subs.sink = this.globalStateService.faqPageActivated$.subscribe((faqPageActivated) => {
      this.faqPageActivated = faqPageActivated;
    });

    this.subs.sink = this.globalStateService.user$.subscribe((user) => {
      this.currentUser = user;
    });

    this.subs.sink = this.globalStateService.webPage$.subscribe((webPage) => {
      this.webPage = webPage;
    });

    this.subs.sink = this.globalStateService.userTeamFetching$.subscribe(() => {
      this.subs.sink = this.appHttpService.getUserTeams().subscribe((res) => {
        if (res.isSuccess) {
          this.userTeamNames = res.data.map((i) => i.name).join(', ');
        }
      });
    });
  }

  login() {
    this.authService.login();
  }

  logout() {
    this.authService.logout(false);
  }
}
