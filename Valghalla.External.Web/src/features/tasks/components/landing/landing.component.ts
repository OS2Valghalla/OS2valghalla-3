import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { TaskOverviewFilterOptions } from '../../models/task-overview-filter-options';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { TaskHttpService } from '../../services/task-http.service';
import { GetTaskOverviewRequest } from '../../models/get-task-overview-request';
import { TaskOverviewItem } from '../../models/task-overview-item';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { isSafari } from 'src/shared/functions/utils';

@Component({
  selector: 'app-tasks-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
  providers: [TaskHttpService],
})
export class TasksLandingComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  readonly webkitDetected: boolean = isSafari();

  loading = true;
  loadingTasks = false;
  filterOptions?: TaskOverviewFilterOptions;
  taskDates: Array<Date>;
  itemsPerPage: number = 12;
  currentPage: number = 1;
  pageCount: number = 0;
  filtersExpanded = false;

  tasks: Array<TaskOverviewItem> = [];
  selectedTask: TaskOverviewItem;

  readonly form = this.formBuilder.group({
    taskTypeId: ['_none'],
    workLocationId: ['_none'],
    teamId: ['_none'],
    selectedDate: [''],
  });

  constructor(
    private readonly router: Router,
    private readonly formBuilder: FormBuilder,
    private readonly taskHttpService: TaskHttpService,
  ) {}

  ngOnInit() {
    this.subs.sink = this.taskHttpService.getTaskOverviewFilterOptions().subscribe((res) => {
      if (res.data) {
        this.filterOptions = res.data;
        this.taskDates = this.filterOptions.taskDates;
        this.getTasks();
      }

      this.loading = false;
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  onFiltersChanged(event) {
    this.getTasks();
  }

  getTasks() {
    this.loadingTasks = true;

    var request: GetTaskOverviewRequest = {
      taskDate: this.form.controls.selectedDate.value ? this.form.controls.selectedDate.value : null,
      workLocationId: this.form.value.workLocationId == '_none' ? undefined : this.form.value.workLocationId,
      taskTypeId: this.form.value.taskTypeId == '_none' ? undefined : this.form.value.taskTypeId,
      teamId: this.form.value.teamId == '_none' ? undefined : this.form.value.teamId,
    };

    this.subs.sink = this.taskHttpService.getTaskOverview(request).subscribe((res) => {
      if (res.isSuccess) {
        this.tasks = res.data;
        this.pageCount = Math.ceil(this.tasks.length / this.itemsPerPage);
      }
      this.loadingTasks = false;
    });
  }

  openTaskDetailsPage(task: TaskOverviewItem) {
    this.router.navigate([RoutingNodes.Tasks, RoutingNodes.TaskDetails, task.hashValue]);
  }

  showRegisterDialog(task: TaskOverviewItem) {
    this.selectedTask = task;
  }

  register() {
    this.router.navigate([RoutingNodes.Registration, RoutingNodes.TaskRegistration, this.selectedTask.hashValue]);
  }

  toggleFiltersExpanded() {
    this.filtersExpanded = !this.filtersExpanded;
  }
}
