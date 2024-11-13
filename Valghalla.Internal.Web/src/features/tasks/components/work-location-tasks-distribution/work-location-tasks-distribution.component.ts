import { Component, OnInit, AfterViewInit, ViewEncapsulation } from '@angular/core';
import { SubSink } from 'subsink';
import { TranslocoService } from '@ngneat/transloco';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { GlobalStateService } from 'src/app/global-state.service';
import { NotificationService } from 'src/shared/services/notification.service';
import { WorkLocationInfo } from '../../models/work-location-info';
import { WorkLocationTasksSummary } from '../../models/work-location-tasks-summary';
import { WorkLocationTasksDistributingRequest, TasksDistributingRequest } from '../../models/work-location-tasks-distributing-request';
import { WorkLocationTasksHttpService } from '../../services/work-location-tasks-http.service';

interface DailyWorkLocationTasks {
  tasksDate: Date;
  teams: Array<TeamTasksSummary>;
}

interface TeamTasksSummary {
  teamId: string;
  teamName: string;
  taskTypes: Array<TaskTypeTasksSummary>;
}

interface TaskTypeTasksSummary {
  taskTypeId: string;
  assignedTasksCount: number;
  allTasksCount: number;
  valueChanged?: boolean;
}

@Component({
  selector: 'app-work-location-tasks-distribution',
  templateUrl: './work-location-tasks-distribution.component.html',
  styleUrls: ['../../../../shared/components/table/table.component.scss', './work-location-tasks-distribution.component.scss'],
  providers: [WorkLocationTasksHttpService],
  encapsulation: ViewEncapsulation.None
})
export class WorkLocationTasksDistributionComponent implements OnInit, AfterViewInit {
    private readonly subs = new SubSink();

    displayedColumns: Array<string> = ['team'];

    workLocation: WorkLocationInfo;

    workLocationTasksSummary: WorkLocationTasksSummary;

    initSelectedDateIndex: number = -1;

    selectedDateIndex: number = 0;

    dailyWorkLocationTasks: Array<DailyWorkLocationTasks> = [];

    itemId: string;

    election?: ElectionShared;

    loading = true;

    submitting = false;

    updatingTasks: Array<TasksDistributingRequest> = [];

    constructor(
        private breadcrumbService: BreadcrumbService,
        private router: Router,
        private route: ActivatedRoute,
        private globalStateService: GlobalStateService,
        private translocoService: TranslocoService,
        private notificationService: NotificationService,
        private workLocationTasksHttpService: WorkLocationTasksHttpService,
    ) {}
      
    ngOnInit(): void {
      this.itemId = this.route.snapshot.paramMap.get(RoutingNodes.WorkLocationId);
      this.subs.sink = this.globalStateService.election$.subscribe((election) => {
        this.election = election;
        this.subs.sink = this.workLocationTasksHttpService.getWorkLocation(this.itemId, this.election.id).subscribe((res) => {            
          this.workLocation = res.data;
          if (this.workLocation) {
              this.breadcrumbService.set('/' + RoutingNodes.TasksOnWorkLocation + '/:' + RoutingNodes.WorkLocationId, this.translocoService.translate('tasks.work_location_tasks.tasks_on') + ' ' + this.workLocation.title);
              this.subs.sink = this.workLocationTasksHttpService.getWorkLocationTasksSummary(this.itemId, this.election.id).subscribe((taskTypesSummaryResult) => {               
                this.workLocationTasksSummary = taskTypesSummaryResult.data;
                this.workLocationTasksSummary.taskTypes.forEach((taskType) => {
                  this.displayedColumns.push(taskType.id);
                });
                this.displayedColumns.push('total');
                this.buildDailyWorkLocationTasks();
              });
          }
          else {
              this.router.navigate([RoutingNodes.TasksOnWorkLocation, this.itemId]);
          }
        });
      });
    }
    
    ngAfterViewInit(): void {
      const loadingInterval =
        setInterval(() => {
          if (this.initSelectedDateIndex > -1) {
            this.onSelectDate(this.initSelectedDateIndex);  
            clearInterval(loadingInterval);
          }
        }, 100);
    }

    buildDailyWorkLocationTasks() {
      const electionDate: Date = new Date(this.workLocationTasksSummary.electionDate);
      const tasksDate: Date = new Date(this.workLocationTasksSummary.electionStartDate);
      while (tasksDate <= new Date(this.workLocationTasksSummary.electionEndDate)) {
        const clonedTasksDate = new Date(tasksDate);        
        var dailyWorkLocationTasksItem: DailyWorkLocationTasks = {
          tasksDate: clonedTasksDate,
          teams: []
        };
        this.workLocationTasksSummary.teams.forEach((team) => {
          const taskTypesSummary: TeamTasksSummary = {
            teamId: team.id,
            teamName: team.name,
            taskTypes: []
          };
          this.workLocationTasksSummary.taskTypes.forEach((taskType) => {
            const foundTasks = this.workLocationTasksSummary.tasks.filter(t => t.teamId == team.id && t.taskTypeId == taskType.id && new Date(t.tasksDate).valueOf() === tasksDate.valueOf());

            const taskTypeTasksSummary: TaskTypeTasksSummary = {
              taskTypeId: taskType.id,
              assignedTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].assignedTasksCount : 0,
              allTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].allTasksCount : 0,            
            };
            taskTypesSummary.taskTypes.push(taskTypeTasksSummary);
          });
          dailyWorkLocationTasksItem.teams.push(taskTypesSummary);
        });
        
        this.dailyWorkLocationTasks.push(dailyWorkLocationTasksItem);
        
        if (tasksDate.valueOf() === electionDate.valueOf()) {
          this.initSelectedDateIndex = this.dailyWorkLocationTasks.indexOf(dailyWorkLocationTasksItem);
        }
        tasksDate.setDate(tasksDate.getDate() + 1);
      }
      this.loading = false;
    }

    getTeamAssignedTasksCount(taskInfo) {
      return taskInfo.taskTypes.map(t => t.assignedTasksCount).reduce((acc, value) => acc + value, 0);
    }

    getTeamAllTasksCount(taskInfo) {
      return taskInfo.taskTypes.map(t => t.allTasksCount).reduce((acc, value) => acc + value, 0);
    }

    getTaskTypeAssignedTasksCount(taskType) {
      const dailyWorkLocationTask = this.dailyWorkLocationTasks[this.selectedDateIndex];
      let assignedTasksCount = 0;
      dailyWorkLocationTask.teams.forEach((team) => {
        const foundTaskTypes = team.taskTypes.filter(t => t.taskTypeId == taskType.id);
        if (foundTaskTypes && foundTaskTypes.length > 0) {
          assignedTasksCount += foundTaskTypes[0].assignedTasksCount;
        }
      })
      return assignedTasksCount;
    }

    getTaskTypeAllTasksCount(taskType) {
      const dailyWorkLocationTask = this.dailyWorkLocationTasks[this.selectedDateIndex];
      let allTasksCount = 0;
      dailyWorkLocationTask.teams.forEach((team) => {
        const foundTaskTypes = team.taskTypes.filter(t => t.taskTypeId == taskType.id);
        if (foundTaskTypes && foundTaskTypes.length > 0) {
          allTasksCount += foundTaskTypes[0].allTasksCount;
        }
      })
      return allTasksCount;
    }

    getSumAssignedTasksCount() {
      const dailyWorkLocationTask = this.dailyWorkLocationTasks[this.selectedDateIndex];
      let assignedTasksCount = 0;
      dailyWorkLocationTask.teams.forEach((team) => {
        team.taskTypes.forEach((taskType) => {
          assignedTasksCount += taskType.assignedTasksCount;
        })
      })
      return assignedTasksCount;
    }

    getSumAllTasksCount() {
      const dailyWorkLocationTask = this.dailyWorkLocationTasks[this.selectedDateIndex];
      let allTasksCount = 0;
      dailyWorkLocationTask.teams.forEach((team) => {
        team.taskTypes.forEach((taskType) => {
          allTasksCount += taskType.allTasksCount;
        })
      })
      return allTasksCount;
    }

    onSelectDate(index) {
      this.selectedDateIndex = index;
    }

    onValueChanged(teamsSummary: TeamTasksSummary, taskTypesSummary: TaskTypeTasksSummary) {      
      if (taskTypesSummary.allTasksCount < taskTypesSummary.assignedTasksCount) {
        taskTypesSummary.allTasksCount = taskTypesSummary.assignedTasksCount;
      }

      const dailyWorkLocationTask = this.dailyWorkLocationTasks[this.selectedDateIndex];
      
      const foundWorkLocationTasks = this.workLocationTasksSummary.tasks.filter(t => t.teamId == teamsSummary.teamId && t.taskTypeId == taskTypesSummary.taskTypeId && (new Date(t.tasksDate)).valueOf() === dailyWorkLocationTask.tasksDate.valueOf());
      
      const foundUpdatingTasks = this.updatingTasks.filter(t => t.teamId == teamsSummary.teamId && t.taskTypeId == taskTypesSummary.taskTypeId && (new Date(t.tasksDate)).valueOf() === dailyWorkLocationTask.tasksDate.valueOf());
      if (!foundWorkLocationTasks || foundWorkLocationTasks.length == 0) {
          if (!foundUpdatingTasks || foundUpdatingTasks.length == 0) {
            if (taskTypesSummary.allTasksCount > 0) {
              this.updatingTasks.push({
                tasksDate: dailyWorkLocationTask.tasksDate,
                taskTypeId : taskTypesSummary.taskTypeId,
                teamId: teamsSummary.teamId,
                tasksCount: taskTypesSummary.allTasksCount
              })
              taskTypesSummary.valueChanged = true;
            }
          }
          else {
            if (taskTypesSummary.allTasksCount > 0) {
              foundUpdatingTasks[0].tasksCount = taskTypesSummary.allTasksCount;
            }
            else {
              this.updatingTasks.splice(this.updatingTasks.indexOf(foundUpdatingTasks[0]), 1);
              taskTypesSummary.valueChanged = false;
            }
          }        
      }
      else {
        if (!foundUpdatingTasks || foundUpdatingTasks.length == 0) {
          this.updatingTasks.push({
            tasksDate: dailyWorkLocationTask.tasksDate,
            taskTypeId : taskTypesSummary.taskTypeId,
            teamId: teamsSummary.teamId,
            tasksCount: taskTypesSummary.allTasksCount
          })
          taskTypesSummary.valueChanged = true;
        }
        else {
          if (taskTypesSummary.allTasksCount === foundWorkLocationTasks[0].allTasksCount) {
            this.updatingTasks.splice(this.updatingTasks.indexOf(foundUpdatingTasks[0]), 1);
            taskTypesSummary.valueChanged = false;
          }
          else {
            foundUpdatingTasks[0].tasksCount = taskTypesSummary.allTasksCount;
          }
        }
      }
    }

    onSave() {      
      if (!this.updatingTasks || this.updatingTasks.length == 0) {
        this.notificationService.showWarning(this.translocoService.translate('tasks.error.no_updated_work_location_tasks'));
      }
      else {
        this.submitting = true;
        const request: WorkLocationTasksDistributingRequest = {
          electionId: this.election.id,
          workLocationId: this.itemId,
          distributingTasks: this.updatingTasks
        }

        this.subs.sink = this.workLocationTasksHttpService.distributeWorkLocationTasks(request).subscribe((res) => {
          if (res.isSuccess) {
            this.router.navigate([RoutingNodes.TasksOnWorkLocation, this.itemId]);
          } else {
            this.submitting = false;
          }
        });
      }
    }

    onCancel() {
      this.router.navigate([RoutingNodes.TasksOnWorkLocation, this.itemId]);
    }

}