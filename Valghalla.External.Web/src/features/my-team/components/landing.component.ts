import { Component, AfterViewInit } from '@angular/core';
import { SubSink } from 'subsink';
import { Clipboard } from '@angular/cdk/clipboard';
import { FormBuilder } from '@angular/forms';
import { TeamHttpService } from '../services/team-http.service';
import { Team } from '../models/team';
import { TeamMember } from '../models/team-member';
import { isSafari } from 'src/shared/functions/utils';
import { TranslocoService } from '@ngneat/transloco';
import { NotificationService } from 'src/shared/services/notification.service';

@Component({
  selector: 'app-my-team-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['landing.component.scss'],
  providers: [TeamHttpService],
})
export class MyTeamLandingComponent implements AfterViewInit {
  private readonly subs = new SubSink();

  loading = true;

  loadingMembers = false;

  teams: Array<Team> = [];

  teamMembers: Array<TeamMember> = [];

  displayTeamMembers: Array<TeamMember> = [];

  generatedLink: string;

  generating: boolean = false;

  itemsPerPage: number = 25;

  currentPage: number = 1;

  pageCount: number = 0;

  selectedTeamMember: TeamMember;

  readonly form = this.formBuilder.group({
    selectedTeamId: [''],
    keyword: '',
  });

  constructor(
    private clipboard: Clipboard,
    private formBuilder: FormBuilder,
    private translocoService: TranslocoService,
    private notificationService: NotificationService,
    private teamHttpService: TeamHttpService,
  ) {}

  ngAfterViewInit() {
    this.subs.sink = this.teamHttpService.getMyTeams().subscribe((res) => {
      if (res.data) {
        this.teams = res.data;
        this.form.controls.selectedTeamId.setValue(this.teams[0].id);
        this.getMembers();
      }
      this.loading = false;
    });
  }

  getMembers() {
    this.loadingMembers = true;
    this.subs.sink = this.teamHttpService.getTeamMembers(this.form.controls.selectedTeamId.value).subscribe((res) => {
      if (res.data) {
        this.teamMembers = res.data;
        this.displayTeamMembers = this.teamMembers;
        this.pageCount = Math.ceil(this.teamMembers.length / this.itemsPerPage);
      }
      this.loadingMembers = false;
    });
  }

  onFilterChanged() {
    this.generatedLink = '';
    this.getMembers();
  }

  showRemoveMemberDialog(member: TeamMember) {
    this.selectedTeamMember = member;
  }

  removeMember() {
    this.loadingMembers = true;

    this.teamHttpService
      .removeTeamMember(this.form.controls.selectedTeamId.value, this.selectedTeamMember.id)
      .subscribe((res) => {
        if (res.isSuccess) {
          this.getMembers();
        } else {
          this.loadingMembers = false;
        }
      });
  }

  ensureTeamLink() {
    if (this.generatedLink && this.generatedLink.length > 0) {
      this.clipboard.copy(this.generatedLink);
      const msg = this.translocoService.translate('my_team.success.create_team_link');
      this.notificationService.showSuccess(msg);
      return;
    }

    this.generatedLink = '';
    this.generating = true;

    this.teamHttpService.createTeamLink(this.form.controls.selectedTeamId.value).subscribe((res) => {
      this.generating = false;

      if (res.isSuccess) {
        this.generatedLink = window.location.origin + res.data;

        if (!this.isSafariBrowser()) {
          this.clipboard.copy(this.generatedLink);
          const msg = this.translocoService.translate('my_team.success.create_team_link');
          this.notificationService.showSuccess(msg);
        }
      }
    });
  }

  isSafariBrowser() {
    return isSafari();
  }

  searchMembers() {
    this.displayTeamMembers = this.teamMembers.filter(
      (t) => t.name.toLowerCase().indexOf(this.form.controls.keyword.value.toLowerCase()) > -1,
    );
    this.pageCount = Math.ceil(this.displayTeamMembers.length / this.itemsPerPage);
  }
}
