import { Component, AfterViewInit, OnDestroy } from '@angular/core';
import { SubSink } from 'subsink';
import { Clipboard } from '@angular/cdk/clipboard';
import { FormBuilder } from '@angular/forms';
import { TeamHttpService } from '../services/team-http.service';
import { Team } from '../models/team';
import { TeamMember } from '../models/team-member';
import { forkJoin, map, interval, fromEvent } from 'rxjs';
import { filter } from 'rxjs/operators';
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

  private readonly autoRefreshIntervalMs = 5000;
  private lastMembersSnapshot = '';

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
        this.setupAutoRefresh();
      }
      this.loading = false;
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

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
        this.snapshotMembers();
        this.loadingMembers = false;
      });
    } else {
      this.subs.sink = this.teamHttpService.getTeamMembers(selected).subscribe((res) => {
        if (res.data) {
          this.teamMembers = res.data;
          this.populateWorkLocationOptions();
          this.applyFilters();
          this.pageCount = Math.ceil(this.teamMembers.length / this.itemsPerPage);
          this.snapshotMembers();
        }
        this.loadingMembers = false;
      });
    }
  }

  private snapshotMembers() {
    try {
      const relevant = this.allTeamsSelected ? this.multiTeamMembersOriginal : this.teamMembers;
      this.lastMembersSnapshot = JSON.stringify(relevant, (key, value) => {
        if (typeof value === 'function') return undefined;
        return value;
      });
    } catch {
    }
  }

  private setupAutoRefresh() {
    this.subs.sink = interval(this.autoRefreshIntervalMs).subscribe(() => {
      if (!this.loadingMembers) {
        this.refreshIfChanged();
      }
    });
    this.subs.sink = fromEvent(document, 'visibilitychange')
      .pipe(filter(() => document.visibilityState === 'visible'))
      .subscribe(() => {
        if (!this.loadingMembers) {
          this.refreshIfChanged(true);
        }
      });
  }

  private refreshIfChanged(force: boolean = false) {
    const prevSnapshot = this.lastMembersSnapshot;
    const selected = this.form.controls.selectedTeamId.value;
    if (selected === 'ALL') {
      const requests = this.teams.map(t => this.teamHttpService.getTeamMembers(t.id).pipe(map(r => ({ team: t, members: r.data || [] }))));
      this.subs.sink = forkJoin(requests).subscribe(results => {
        const newSnapshot = JSON.stringify(results);
        if (force || newSnapshot !== prevSnapshot) {
          this.multiTeamMembersOriginal = results;
          this.teamMembers = results.flatMap(r => r.members);
          this.populateWorkLocationOptions();
          this.applyFilters();
          this.lastMembersSnapshot = newSnapshot;
        }
      });
    } else {
      this.subs.sink = this.teamHttpService.getTeamMembers(selected).subscribe(res => {
        const members = res.data || [];
        const newSnapshot = JSON.stringify(members);
        if (force || newSnapshot !== prevSnapshot) {
          this.teamMembers = members;
          this.populateWorkLocationOptions();
          this.applyFilters();
          this.lastMembersSnapshot = newSnapshot;
        }
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
    const keyword = (this.form.controls.keyword.value || '').toLowerCase();
    const wl = this.form.controls.workLocation.value;
    const statusFilter = this.form.controls.taskStatus.value;
    const dateFilter = this.form.controls.taskDate.value; // yyyy-MM-dd
    const statusNumber = statusFilter === '' || statusFilter === null ? null : +statusFilter;

    const filterTasksByStatus = (members: TeamMember[]): TeamMember[] => {
      if (statusNumber === null || statusNumber === undefined) return members;
      return members
        .map(m => {
          const filteredWorkLocations = (m.workLocations || [])
            .map(w => ({
              ...w,
              tasks: (w.tasks || []).filter(t => t.taskStatus === statusNumber && (!dateFilter || (t.taskDate && t.taskDate.startsWith(dateFilter)))),
            }))
            .filter(w => w.tasks.length > 0);
          return { ...m, workLocations: filteredWorkLocations } as TeamMember;
        })
        .filter(m => m.workLocations && m.workLocations.length > 0);
    };
    if (this.allTeamsSelected) {
      this.multiTeamMembersFiltered = this.multiTeamMembersOriginal
        .map(grp => {
          let members = grp.members.filter(member => {
            const matchesName = !keyword || member.name.toLowerCase().includes(keyword);
            const matchesWl = !wl || member.workLocations?.some(w => w.workLocationTitle === wl);
            const matchesDate = !dateFilter || member.workLocations?.some(w => w.tasks?.some(t => t.taskDate && t.taskDate.startsWith(dateFilter)));
            return matchesName && matchesWl && matchesDate;
          });
          members = filterTasksByStatus(members);
          return { team: grp.team, members };
        })
        .filter(grp => grp.members.length > 0);
    } else {
      let filtered = this.teamMembers.filter(member => {
        const matchesName = !keyword || member.name.toLowerCase().includes(keyword);
        const matchesWl = !wl || member.workLocations?.some(w => w.workLocationTitle === wl);
        const matchesDate = !dateFilter || member.workLocations?.some(w => w.tasks?.some(t => t.taskDate && t.taskDate.startsWith(dateFilter)));
        return matchesName && matchesWl && matchesDate;
      });
      filtered = filterTasksByStatus(filtered);
      this.displayTeamMembers = filtered;
      this.currentPage = 1;
      this.pageCount = Math.ceil(this.displayTeamMembers.length / this.itemsPerPage);
    }
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
