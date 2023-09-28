import { Component, AfterViewInit } from '@angular/core';
import { SubSink } from 'subsink';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { FormBuilder } from '@angular/forms';
import { WorkLocationShared } from 'src/shared/models/work-location-shared';
import { UnprotectedTasksHttpService } from '../../services/unprotected-tasks-http.service';
import { GetUnprotectedAvailableTasksByFiltersRequest } from '../../models/get-unprotected-available-tasks-by-filters-request';
import { AvailableTasksDetails } from '../../models/available-tasks-details';
import { isSafari } from 'src/shared/functions/utils';

@Component({
  selector: 'app-filtered-tasks',
  templateUrl: './filtered-tasks.component.html',
  styleUrls: ['./filtered-tasks.component.scss'],
  providers: [UnprotectedTasksHttpService],
})
export class FilteredTasksComponent implements AfterViewInit {
  private readonly subs = new SubSink();

  readonly webkitDetected: boolean = isSafari();

  loading = true;

  loadingTasks = false;

  hashValue: string;

  taskDates: Array<Date>;

  workLocations: Array<WorkLocationShared> = [];

  tasks: Array<AvailableTasksDetails> = [];

  itemsPerPage: number = 12;

  currentPage: number = 1;

  pageCount: number = 0;

  filtersExpanded = false;

  selectedTask: AvailableTasksDetails;

  readonly form = this.formBuilder.group({
    selectedWorkLocationId: [''],
    selectedDate: [''],
  });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private unprotectedTasksHttpService: UnprotectedTasksHttpService,
  ) {}

  ngAfterViewInit() {
    this.hashValue = this.route.snapshot.queryParamMap.get(RoutingNodes.Id);
    this.subs.sink = this.unprotectedTasksHttpService.getTasksFiltersOptions(this.hashValue).subscribe((res) => {
      if (res.data) {
        this.workLocations = res.data.workLocations;

        this.taskDates = res.data.taskDates;

        if (res.data.tasksFilter.workLocationIds && res.data.tasksFilter.workLocationIds.length > 0) {
          this.form.controls.selectedWorkLocationId.setValue(res.data.tasksFilter.workLocationIds[0]);
        }
        if (res.data.tasksFilter.taskDate) {
          this.taskDates.forEach(taskDate => {
            if ((new Date(taskDate)).valueOf() == (new Date(res.data.tasksFilter.taskDate)).valueOf()){
              this.form.controls.selectedDate.setValue(taskDate.toString());
            }
          });
        }
        this.getTasks();
      }
      this.loading = false;
    });
  }

  onFiltersChanged(event) {
    this.getTasks();
  }

  getTasks() {
    this.loadingTasks = true;
    var request: GetUnprotectedAvailableTasksByFiltersRequest = {
      hashValue: this.hashValue,
      tasksFilter: {
        workLocationIds: this.form.controls.selectedWorkLocationId.value
          ? [this.form.controls.selectedWorkLocationId.value]
          : [],
        taskDate: this.form.controls.selectedDate.value ? this.form.controls.selectedDate.value : null,
      },
    };

    this.subs.sink = this.unprotectedTasksHttpService.getAvailableTasksByFilters(request).subscribe((res) => {
      if (res.isSuccess) {
        this.tasks = res.data;
        this.pageCount = Math.ceil(this.tasks.length / this.itemsPerPage);
      }
      this.loadingTasks = false;
    });
  }

  openTaskDetailsPage(task: AvailableTasksDetails) {
    this.router.navigate([RoutingNodes.Tasks, RoutingNodes.TaskDetails, task.hashValue]);
  }

  toggleFiltersExpanded() {
    this.filtersExpanded = !this.filtersExpanded;
  }

  showRegisterDialog(task: AvailableTasksDetails) {
    this.selectedTask = task;
  }

  register() {
    this.router.navigate([RoutingNodes.Registration, RoutingNodes.TaskRegistration, this.selectedTask.hashValue]);
  }
}
