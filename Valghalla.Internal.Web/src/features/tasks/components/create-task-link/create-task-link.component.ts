import { Component, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { Clipboard } from '@angular/cdk/clipboard';
import { TranslocoService } from '@ngneat/transloco';
import { CreateFilteredTaskLinkRequest, TasksFilterRequest } from 'src/features/administration/link/models/create-filtered-tasks-link-request';
import { NotificationService } from 'src/shared/services/notification.service';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { GlobalStateService } from 'src/app/global-state.service';
import { AreaShared } from 'src/shared/models/area/area-shared';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { LinkHttpService } from 'src/features/administration/link/services/link-http.service';
import { WorkLocationWithTeamIdsResponse, TaskTypeWithTeamIdsResponse } from '../../models/tasks-filters-options';
import { GetAvailableTasksByFiltersRequest } from '../../models/get-available-tasks-by-filters-request';
import { AvailableTasksDetails } from '../../models/available-tasks-details';
import { FilteredTasksHttpService } from '../../services/filtered-tasks-http.service';

@Component({
  selector: 'app-create-task-link',
  templateUrl: './create-task-link.component.html',
  providers: [FilteredTasksHttpService, LinkHttpService]
})
export class CreateTaskLinkComponent implements OnInit {
    private readonly subs = new SubSink();

    election?: ElectionShared;

    loading = true;

    generatingLink = false;

    loadingTasks = false;

    electionDates: Array<Date> = [];

    areas: Array<AreaShared> = [];

    teams: Array<TeamShared> = [];

    workLocations: Array<WorkLocationWithTeamIdsResponse> = [];

    displayWorkLocations: Array<WorkLocationWithTeamIdsResponse> = [];

    taskTypes: Array<TaskTypeWithTeamIdsResponse> = [];

    displayTaskTypes: Array<TaskTypeWithTeamIdsResponse> = [];

    selectedDate?: Date;

    selectedAreaIds?: Array<string> = [];

    selectedTeamId?: string;

    selectedWorkLocationIds?: Array<string> = [];

    selectedTaskTypeIds?: Array<string> = [];

    trustedTaskType: boolean;

    tasksLink: string;

    tasks: Array<AvailableTasksDetails> = [];

    constructor(
        private clipboard: Clipboard,
        private globalStateService: GlobalStateService,
        private translocoService: TranslocoService,
        private notificationService: NotificationService,
        private linkHttpService: LinkHttpService,
        private filteredTasksHttpService: FilteredTasksHttpService
    ) {}

    ngOnInit(): void {
        this.subs.sink = this.globalStateService.election$.subscribe((election) => {
            this.election = election;
            if (this.election) {
                this.subs.sink = this.filteredTasksHttpService.getTasksFiltersOptions(this.election.id).subscribe((res) => {
                    this.areas = res.data.areas;
                    this.teams = res.data.teams;
                    this.workLocations = res.data.workLocations;
                    this.taskTypes = res.data.taskTypes;
                    if (this.teams && this.teams.length > 0) {
                        this.selectedTeamId = this.teams[0].id;
                        this.displayWorkLocations = this.workLocations.filter(t => t.teamIds.indexOf(this.selectedTeamId) > -1);
                        this.displayTaskTypes = this.taskTypes.filter(t => t.teamIds.indexOf(this.selectedTeamId) > -1);                    
                    }
                    var tasksDate: Date = new Date(res.data.electionStartDate);
                    while (tasksDate <= new Date(res.data.electionEndDate)) {
                        this.electionDates.push(new Date(tasksDate));
                        tasksDate.setDate(tasksDate.getDate() + 1);
                    }

                    this.onFilterChanged();
                    this.loading = false;
                });
            }
        });
    }

    onTeamFilterChanged() {
        this.selectedWorkLocationIds = [];
        this.selectedTaskTypeIds = [];
        this.displayWorkLocations = this.workLocations.filter(t => t.teamIds.indexOf(this.selectedTeamId) > -1);
        this.displayTaskTypes = this.taskTypes.filter(t => t.teamIds.indexOf(this.selectedTeamId) > -1);  
        this.onFilterChanged();      
    }

    onFilterChanged() {
        this.loadingTasks = true;
        this.tasksLink = '';

        var request: GetAvailableTasksByFiltersRequest = {
            electionId: this.election.id,
            tasksFilter: {
                teamId: this.selectedTeamId,
                areaIds: [],
                workLocationIds: [],
                taskTypeIds: [],
                taskDate: this.selectedDate ? new Date(this.selectedDate) : null,
                trustedTaskType: this.trustedTaskType
            }
        };

        if (this.selectedAreaIds && this.selectedAreaIds.length > 0) {
            request.tasksFilter.areaIds = this.selectedAreaIds;
        }
        if (this.selectedWorkLocationIds && this.selectedWorkLocationIds.length > 0) {
            request.tasksFilter.workLocationIds = this.selectedWorkLocationIds;
        }
        if (this.selectedTaskTypeIds && this.selectedTaskTypeIds.length > 0) {
            request.tasksFilter.taskTypeIds = this.selectedTaskTypeIds;
        }
        
        this.subs.sink = this.filteredTasksHttpService.getAvailableTasksByFilters(request).subscribe((res) => {
            this.tasks = res.data;
            this.loadingTasks = false;
        });
    }

    copyTaskDetailsLink(taskDetailsPageUrl: string) {
        this.clipboard.copy(taskDetailsPageUrl);
        this.notificationService.showSuccess(this.translocoService.translate('tasks.success.task_link_copied'));
    }

    copyFilteredTasksLink() {
        this.generatingLink = true;    

        var filters: TasksFilterRequest = {
            teamId: this.selectedTeamId,
            areaIds: [],
            workLocationIds: [],
            taskTypeIds: [],
            taskDate: this.selectedDate ? new Date(this.selectedDate) : null,
            trustedTaskType: this.trustedTaskType
        };

        if (this.selectedAreaIds && this.selectedAreaIds.length > 0) {
            filters.areaIds = this.selectedAreaIds.sort();
        }
        if (this.selectedWorkLocationIds && this.selectedWorkLocationIds.length > 0) {
            filters.workLocationIds = this.selectedWorkLocationIds.sort();
        }
        if (this.selectedTaskTypeIds && this.selectedTaskTypeIds.length > 0) {
            filters.taskTypeIds = this.selectedTaskTypeIds.sort();
        }

        var request: CreateFilteredTaskLinkRequest = {
            electionId: this.election.id,
            tasksFilter: filters
        };

        this.subs.sink = this.linkHttpService.createFilteredTasksLink(request).subscribe((res) => {
            if (res.isSuccess) {
                this.tasksLink = res.data;
                this.clipboard.copy(res.data);
                this.generatingLink = false;
                this.notificationService.showSuccess(this.translocoService.translate('tasks.success.filtered_tasks_link_copied'));
            } else {
                this.generatingLink = false;
            }
        }); 
    }
}