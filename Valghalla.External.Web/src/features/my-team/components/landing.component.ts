import { Component, AfterViewInit, OnDestroy } from '@angular/core';
import { SubSink } from 'subsink';
import { Clipboard } from '@angular/cdk/clipboard';
import { FormBuilder } from '@angular/forms';
import { TeamHttpService } from '../services/team-http.service';
import { Team } from '../models/team';
import { TeamMember } from '../models/team-member';
import { forkJoin, map } from 'rxjs';
import { isSafari } from 'src/shared/functions/utils';
import { TranslocoService } from '@ngneat/transloco';
import { NotificationService } from 'src/shared/services/notification.service';

@Component({
  selector: 'app-my-team-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['landing.component.scss'],
  providers: [TeamHttpService],
})
export class MyTeamLandingComponent implements AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading = true;

  loadingMembers = false;

  teams: Array<Team> = [];

  teamMembers: Array<TeamMember> = [];

  displayTeamMembers: Array<TeamMember> = [];

  multiTeamMembersOriginal: { team: Team; members: TeamMember[] }[] = [];
  multiTeamMembersFiltered: { team: Team; members: TeamMember[] }[] = [];

  generatedLink: string;

  generating: boolean = false;

  itemsPerPage: number = 25;

  currentPage: number = 1;

  pageCount: number = 0;

  selectedTeamMember: TeamMember;

  readonly form = this.formBuilder.group({
    selectedTeamId: [''],
    keyword: '',
    workLocation: [''],
    taskStatus: [''],
    taskDate: [''],
  });

  workLocationOptions: string[] = [];
  taskDateOptions: string[] = [];

  constructor(
    private clipboard: Clipboard,
    private formBuilder: FormBuilder,
    private translocoService: TranslocoService,
    private notificationService: NotificationService,
    private teamHttpService: TeamHttpService,
  ) { }

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
  ngOnDestroy(): void { this.subs.unsubscribe(); }
  get allTeamsSelected(): boolean {
    return this.form.controls.selectedTeamId.value === 'ALL';
  }

  getMembers() {
    this.loadingMembers = true;
    const selected = this.form.controls.selectedTeamId.value;

    if (selected === 'ALL') {
      const requests = this.teams.map(t => this.teamHttpService.getTeamMembers(t.id).pipe(map(r => ({ team: t, members: r.data || [] }))));
      this.subs.sink = forkJoin(requests).subscribe(results => {
        this.multiTeamMembersOriginal = results;
        this.teamMembers = results.flatMap(r => r.members);
        this.populateWorkLocationOptions();
        this.applyFilters();
        this.loadingMembers = false;
      });
    } else {
      this.subs.sink = this.teamHttpService.getTeamMembers(selected).subscribe((res) => {
        if (res.data) {
          this.teamMembers = res.data;
          this.populateWorkLocationOptions();
          this.applyFilters();
          this.pageCount = Math.ceil(this.teamMembers.length / this.itemsPerPage);
        }
        this.loadingMembers = false;
      });
    }
  }

  private populateWorkLocationOptions() {
    const set = new Set<string>();
    this.teamMembers.forEach(m => m.workLocations?.forEach(wl => set.add(wl.workLocationTitle)));
    this.workLocationOptions = Array.from(set.values()).sort();
    const dateSet = new Set<string>();
    this.teamMembers.forEach(m => m.workLocations?.forEach(wl => wl.tasks?.forEach(t => { if (t.taskDate) { dateSet.add(t.taskDate.substring(0, 10)); } })));
    this.taskDateOptions = Array.from(dateSet.values()).sort();
  }

  applyFilters() {
    const { keyword, workLocation, taskStatus, taskDate } = this.form.value;
    const k = (keyword || '').toLowerCase();
    const wl = workLocation as string;
    const dateFilter = taskDate as string;
    const statusNumber = taskStatus === '' || taskStatus === null ? null : Number(taskStatus);
    const process = (members: TeamMember[]) => this.pruneMembers(this.filterMembers(members, k, wl, dateFilter), statusNumber, dateFilter);
    if (this.allTeamsSelected) {
      this.multiTeamMembersFiltered = this.multiTeamMembersOriginal
        .map(g => ({ team: g.team, members: process(g.members) }))
        .filter(g => g.members.length);
    } else {
      this.displayTeamMembers = process(this.teamMembers);
      this.currentPage = 1;
      this.pageCount = Math.ceil(this.displayTeamMembers.length / this.itemsPerPage);
    }
  }

  private filterMembers(members: TeamMember[], keyword: string, wl: string, date: string): TeamMember[] {
    return members.filter(m => {
      const nameOk = !keyword || m.name.toLowerCase().includes(keyword);
      const wlOk = !wl || m.workLocations?.some(w => w.workLocationTitle === wl);
      const dateOk = !date || m.workLocations?.some(w => w.tasks?.some(t => t.taskDate && t.taskDate.startsWith(date)));
      return nameOk && wlOk && dateOk;
    });
  }

  private pruneMembers(members: TeamMember[], statusNumber: number | null, date: string): TeamMember[] {
    if (statusNumber === null && !date) return members;
    return members
      .map(m => {
        const workLocations = (m.workLocations || [])
          .map(w => {
            let tasks = w.tasks || [];
            if (statusNumber !== null) tasks = tasks.filter(t => t.taskStatus === statusNumber);
            if (date) tasks = tasks.filter(t => t.taskDate && t.taskDate.startsWith(date));
            return { ...w, tasks };
          })
          .filter(w => w.tasks.length);
        return { ...m, workLocations } as TeamMember;
      })
      .filter(m => m.workLocations && m.workLocations.length);
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
    this.applyFilters();
  }
}
