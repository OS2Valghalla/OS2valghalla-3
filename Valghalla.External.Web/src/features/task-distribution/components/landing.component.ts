import { Component, AfterViewInit } from '@angular/core';
import { SubSink } from 'subsink';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { FormBuilder } from '@angular/forms';
import { TeamResponsibleTaskHttpService } from '../services/team-responsible-task-http.service';
import {
  TeamResponsibleTaskTeam,
  TeamResponsibleTaskWorkLocation,
  TeamResponsibleTaskTaskType,
  TeamResponsibleTaskDetails,
} from '../models/team-responsible-tasks';
import { GetTeamResponsibleTasksRequest } from '../models/get-team-responsible-tasks-request';
import { isSafari } from 'src/shared/functions/utils';

@Component({
  selector: 'app-task-distribution-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
  providers: [TeamResponsibleTaskHttpService],
})
export class TaskDistributionLandingComponent implements AfterViewInit {
  private readonly subs = new SubSink();

  readonly webkitDetected: boolean = isSafari();

  loading = true;

  loadingTasks = false;

  minDateText: string;

  workLocations: Array<TeamResponsibleTaskWorkLocation> = [];

  teams: Array<TeamResponsibleTaskTeam> = [];

  taskTypes: Array<TeamResponsibleTaskTaskType> = [];

  tasks: Array<TeamResponsibleTaskDetails> = [];

  itemsPerPage: number = 12;

  currentPage: number = 1;

  pageCount: number = 0;

  selectedTaskDate: Date = undefined;

  filtersExpanded = false;

  totalAcceptedTasksCount: number = 0;

  totalTasksCount: number = 0;

  selectedTask: TeamResponsibleTaskDetails;

  readonly form = this.formBuilder.group({
    selectedWorkLocationId: [''],
    selectedTeamId: [''],
    selectedTaskTypeId: [''],
    selectedDate: [undefined as any],
  });

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private taskHttpService: TeamResponsibleTaskHttpService,
  ) {}

  ngAfterViewInit() {
    var currentDate = new Date();
    currentDate.setDate(currentDate.getDate() + 1);
    this.minDateText = currentDate.toISOString();
    this.subs.sink = this.taskHttpService.getTeamResponsibleTasksFiltersOptions().subscribe((res) => {
      if (res.data) {
        this.teams = res.data.teams;
        this.workLocations = res.data.workLocations;
        this.taskTypes = res.data.taskTypes;
        if (this.teams.length > 0) {
          this.form.controls.selectedTeamId.setValue(this.teams[0].id);
          this.getTasks();
        }
      }
      this.loading = false;
    });
  }

  onFilterChanged(event) {
    this.getTasks();
  }

  onDateChanged(event) {
    if (!event.target.value) {
      if (this.selectedTaskDate) {
        this.selectedTaskDate = undefined;
        this.getTasks();
      }
    } else if (!this.selectedTaskDate || this.selectedTaskDate.valueOf() != new Date(event.target.value).valueOf()) {
      var tzOffset = new Date().getTimezoneOffset() * 60000;

      this.selectedTaskDate = new Date(new Date(event.target.value).valueOf() + tzOffset);
      this.getTasks();
    }
  }

  getTasks() {
    this.loadingTasks = true;
    var request: GetTeamResponsibleTasksRequest = {
      teamId: this.form.controls.selectedTeamId.value ? this.form.controls.selectedTeamId.value : null,
      workLocationId: this.form.controls.selectedWorkLocationId.value
        ? this.form.controls.selectedWorkLocationId.value
        : null,
      taskTypeId: this.form.controls.selectedTaskTypeId.value ? this.form.controls.selectedTaskTypeId.value : null,
      taskDate: this.selectedTaskDate ? this.selectedTaskDate : null,
    };

    this.subs.sink = this.taskHttpService.getTeamResponsibleTasks(request).subscribe((res) => {
      if (res.isSuccess) {
        this.tasks = res.data.tasks;
        this.totalAcceptedTasksCount = res.data.totalAcceptedTasksCount;
        this.totalTasksCount = res.data.totalTasksCount;
        this.pageCount = Math.ceil(this.tasks.length / this.itemsPerPage);
      }
      this.loadingTasks = false;
    });
  }

  openTaskDetailsPage(task: TeamResponsibleTaskDetails) {
    this.router.navigate([RoutingNodes.TaskDistribution, RoutingNodes.TaskDetails, task.hashValue]);
  }

  showRegisterDialog(task: TeamResponsibleTaskDetails) {
    this.selectedTask = task;
  }

  register() {
    this.router.navigate([RoutingNodes.Registration, RoutingNodes.TaskRegistration, this.selectedTask.hashValue]);
  }

  toggleFiltersExpanded() {
    this.filtersExpanded = !this.filtersExpanded;
  }
}
