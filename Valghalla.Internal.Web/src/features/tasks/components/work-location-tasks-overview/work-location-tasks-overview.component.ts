import { Component, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { TranslocoService } from '@ngneat/transloco';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { Clipboard } from '@angular/cdk/clipboard';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { GlobalStateService } from 'src/app/global-state.service';
import { NotificationService } from 'src/shared/services/notification.service';
import { ConfirmationDialogComponent } from 'src/shared/components/confirmation-dialog/confirmation-dialog.component';
import { LinkHttpService } from 'src/features/administration/link/services/link-http.service';
import { WorkLocationInfo } from '../../models/work-location-info';
import { WorkLocationTasksSummary } from '../../models/work-location-tasks-summary';
import { TaskAssignment } from '../../models/task-assignment';
import { AssignParticipantToTaskRequest } from '../../models/assign-participant-to-task-request';
import { RemoveParticipantFromTaskRequest } from '../../models/remove-participant-from-task-request';
import { WorkLocationTasksHttpService } from '../../services/work-location-tasks-http.service';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { MoveTasksRequest } from '../../models/move-tasks-request';

interface UpdatingTaskAssignment extends TaskAssignment {
  isUpdating?: boolean;
}

interface DailyWorkLocationTasks {
  index?: number;
  tasksDate?: Date;
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
  awaitingTasksCount: number;
  allTasksCount: number;
}

interface DailyTeamTasks {
  index?: number;
  teamId: string;
  loadingTasks: boolean;
  tasks: Array<UpdatingTaskAssignment>;
  displayingTasks: Array<UpdatingTaskAssignment>;
  loadingRejectedTasks: boolean;
  rejectedTasks: Array<TaskAssignment>;
  rejectedDisplayingTasks: Array<TaskAssignment>;
}

@Component({
  selector: 'app-work-location-tasks-overview',
  templateUrl: './work-location-tasks-overview.component.html',
  styleUrls: ['../../../../shared/components/table/table.component.scss', 'work-location-tasks-overview.component.scss'],
  providers: [WorkLocationTasksHttpService, LinkHttpService]
})
export class WorkLocationTasksOverviewComponent implements OnInit {

  private readonly subs = new SubSink();

  displayedColumns: Array<string> = ['team'];

  teamTasksColumns: Array<string> = ['status', 'taskType', 'date', 'participant', 'actions'];

  rejectedTeamTasksColumns: Array<string> = ['status', 'taskType', 'date', 'participant', 'rejectedDate', 'actions'];

  workLocation: WorkLocationInfo;

  workLocationTasksSummary: WorkLocationTasksSummary;

  dailyWorkLocationTasks: Array<DailyWorkLocationTasks> = [];

  allDatesWorkLocationTasks: DailyWorkLocationTasks;

  dailyTeamTasks: Array<DailyTeamTasks> = [];

  selectedDate: any;

  itemId: string;

  election?: ElectionShared;

  loading = true;

  openAddParticipant = false;

  invalidWorkLocationId = false;

  selectedDateIndex = -1;

  assigningTask: UpdatingTaskAssignment;

  selectedTeamIndex: number;

  selectedTeam: TeamShared;

  targetTeamId: any;

  readonly form = this.formBuilder.group({
    selectedParticipantIds: [[] as string[]],
  });

  constructor(
    private dialog: MatDialog,
    private breadcrumbService: BreadcrumbService,
    private translocoService: TranslocoService,
    private router: Router,
    private route: ActivatedRoute,
    private clipboard: Clipboard,
    private globalStateService: GlobalStateService,
    private notificationService: NotificationService,
    private formBuilder: FormBuilder,
    private workLocationTasksHttpService: WorkLocationTasksHttpService,
    private linkHttpService: LinkHttpService,
  ) { }

  ngOnInit(): void {
    this.itemId = this.route.snapshot.paramMap.get(RoutingNodes.WorkLocationId);
    this.subs.sink = this.globalStateService.election$.subscribe((election) => {
      this.election = election;
      this.subs.sink = this.workLocationTasksHttpService.getWorkLocation(this.itemId, this.election.id).subscribe((res) => {
        this.workLocation = res.data;
        if (this.workLocation) {
          this.invalidWorkLocationId = false;
          this.breadcrumbService.set('/' + RoutingNodes.TasksOnWorkLocation + '/:' + RoutingNodes.WorkLocationId, this.translocoService.translate('tasks.work_location_tasks.tasks_on') + ' ' + this.workLocation.title);
          this.subs.sink = this.workLocationTasksHttpService.getWorkLocationTasksSummary(this.itemId, this.election.id).subscribe((taskTypesSummaryResult) => {
            this.workLocationTasksSummary = taskTypesSummaryResult.data;
            this.workLocationTasksSummary.taskTypes.forEach((taskType) => {
              this.displayedColumns.push(taskType.id);
            });
            this.workLocationTasksSummary.teams.forEach((team, index) => {
              this.dailyTeamTasks.push({ index: index, teamId: team.id, tasks: [], displayingTasks: [], rejectedTasks: [], rejectedDisplayingTasks: [], loadingTasks: true, loadingRejectedTasks: true });
            });
            this.displayedColumns.push('total');
            this.buildWorkLocationTasksSummary();
            this.buildTeamTasksSummary();
            this.buildRejectedTeamTasksSummary();
          });
        }
        else {
          this.invalidWorkLocationId = true;
          this.breadcrumbService.set('/' + RoutingNodes.TasksOnWorkLocation + '/:' + RoutingNodes.WorkLocationId, '');
        }
      });
    });
  }

  buildWorkLocationTasksSummary() {
    this.allDatesWorkLocationTasks = {
      teams: []
    };

    const tasksDate: Date = new Date(this.workLocationTasksSummary.electionEndDate);
    let index = 0;
    while (tasksDate >= new Date(this.workLocationTasksSummary.electionStartDate)) {
      const clonedTasksDate = new Date(tasksDate);
      var dailyWorkLocationTasksItem: DailyWorkLocationTasks = {
        tasksDate: clonedTasksDate,
        index: index,
        teams: []
      };
      this.workLocationTasksSummary.teams.forEach((team, teamIndex) => {
        const taskTypesSummary: TeamTasksSummary = {
          teamId: team.id,
          teamName: team.name,
          taskTypes: []
        };
        if (this.allDatesWorkLocationTasks.teams.length <= teamIndex) {
          const allDatesTaskTypesSummary: TeamTasksSummary = {
            teamId: team.id,
            teamName: team.name,
            taskTypes: []
          };
          this.allDatesWorkLocationTasks.teams.push(allDatesTaskTypesSummary);
        }
        this.workLocationTasksSummary.taskTypes.forEach((taskType, taskTypeIndex) => {
          const foundTasks = this.workLocationTasksSummary.tasks.filter(t => t.teamId == team.id && t.taskTypeId == taskType.id && new Date(t.tasksDate).valueOf() === tasksDate.valueOf());

          const taskTypeTasksSummary: TaskTypeTasksSummary = {
            taskTypeId: taskType.id,
            assignedTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].assignedTasksCount : 0,
            awaitingTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].awaitingTasksCount : 0,
            allTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].allTasksCount : 0,
          };
          taskTypesSummary.taskTypes.push(taskTypeTasksSummary);

          if (this.allDatesWorkLocationTasks.teams[teamIndex].taskTypes.length <= taskTypeIndex) {
            const allDatesTaskTypeTasksSummary: TaskTypeTasksSummary = {
              taskTypeId: taskType.id,
              assignedTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].assignedTasksCount : 0,
              awaitingTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].awaitingTasksCount : 0,
              allTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].allTasksCount : 0,
            };
            this.allDatesWorkLocationTasks.teams[teamIndex].taskTypes.push(allDatesTaskTypeTasksSummary);
          }
          else if (foundTasks && foundTasks.length > 0) {
            this.allDatesWorkLocationTasks.teams[teamIndex].taskTypes[taskTypeIndex].assignedTasksCount += foundTasks[0].assignedTasksCount;
            this.allDatesWorkLocationTasks.teams[teamIndex].taskTypes[taskTypeIndex].awaitingTasksCount += foundTasks[0].awaitingTasksCount;
            this.allDatesWorkLocationTasks.teams[teamIndex].taskTypes[taskTypeIndex].allTasksCount += foundTasks[0].allTasksCount;
          }
        });
        dailyWorkLocationTasksItem.teams.push(taskTypesSummary);
      });

      this.dailyWorkLocationTasks.push(dailyWorkLocationTasksItem);
      tasksDate.setDate(tasksDate.getDate() - 1);
      index++;
    }

    if (this.selectedDateIndex >= 0) {
      this.dailyTeamTasks.forEach((team) => {
        team.displayingTasks = team.tasks.filter(t => (new Date(t.taskDate)).valueOf() === this.dailyWorkLocationTasks[this.selectedDateIndex].tasksDate.valueOf());
        team.rejectedDisplayingTasks = team.rejectedTasks.filter(t => (new Date(t.taskDate)).valueOf() === this.dailyWorkLocationTasks[this.selectedDateIndex].tasksDate.valueOf());
      });
    }

    this.loading = false;
  }

  buildTeamTasksSummary() {
    this.dailyTeamTasks.forEach((team) => {
      this.workLocationTasksHttpService.getTeamTasks(team.teamId, this.itemId, this.election.id, false).subscribe((teamTasksResult) => {
        team.tasks = teamTasksResult.data;
        team.displayingTasks = teamTasksResult.data;
        team.loadingTasks = false;
      });
    });
  }

  buildRejectedTeamTasksSummary() {
    this.dailyTeamTasks.forEach((team) => {
      this.workLocationTasksHttpService.getTeamTasks(team.teamId, this.itemId, this.election.id, true).subscribe((teamTasksResult) => {
        team.rejectedTasks = teamTasksResult.data;
        team.rejectedDisplayingTasks = teamTasksResult.data;
        team.loadingRejectedTasks = false;
      });
    });
  }

  onDateChanged() {
    if (this.selectedDateIndex >= 0) {
      this.dailyTeamTasks.forEach((team) => {
        team.displayingTasks = team.tasks.filter(t => (new Date(t.taskDate)).valueOf() === this.dailyWorkLocationTasks[this.selectedDateIndex].tasksDate.valueOf());
        team.rejectedDisplayingTasks = team.rejectedTasks.filter(t => (new Date(t.taskDate)).valueOf() === this.dailyWorkLocationTasks[this.selectedDateIndex].tasksDate.valueOf());
      });
    }
    else {
      this.dailyTeamTasks.forEach((team) => {
        team.displayingTasks = team.tasks;
        team.rejectedDisplayingTasks = team.rejectedTasks;
      });
    }
  }

  getTeamAssignedTasksCount(taskInfo) {
    return taskInfo.taskTypes.map(t => t.assignedTasksCount).reduce((acc, value) => acc + value, 0);
  }

  getTeamAllTasksCount(taskInfo) {
    return taskInfo.taskTypes.map(t => t.allTasksCount).reduce((acc, value) => acc + value, 0);
  }

  getTaskTypeAssignedTasksCount(taskType) {
    const dailyWorkLocationTask = this.selectedDateIndex > - 1 ? this.dailyWorkLocationTasks[this.selectedDateIndex] : this.allDatesWorkLocationTasks;
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
    const dailyWorkLocationTask = this.selectedDateIndex > - 1 ? this.dailyWorkLocationTasks[this.selectedDateIndex] : this.allDatesWorkLocationTasks;
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
    const dailyWorkLocationTask = this.selectedDateIndex > - 1 ? this.dailyWorkLocationTasks[this.selectedDateIndex] : this.allDatesWorkLocationTasks;
    let assignedTasksCount = 0;
    dailyWorkLocationTask.teams.forEach((team) => {
      team.taskTypes.forEach((taskType) => {
        assignedTasksCount += taskType.assignedTasksCount;
      })
    })
    return assignedTasksCount;
  }

  getSumAllTasksCount() {
    const dailyWorkLocationTask = this.selectedDateIndex > - 1 ? this.dailyWorkLocationTasks[this.selectedDateIndex] : this.allDatesWorkLocationTasks;
    let allTasksCount = 0;
    dailyWorkLocationTask.teams.forEach((team) => {
      team.taskTypes.forEach((taskType) => {
        allTasksCount += taskType.allTasksCount;
      })
    })
    return allTasksCount;
  }

  openDistributeTasks() {
    this.router.navigate([RoutingNodes.TasksOnWorkLocation, this.itemId, RoutingNodes.Link_Distribute]);
  }

  openParticipantEditing(participantId: string) {
    this.router.navigate([RoutingNodes.Participant, RoutingNodes.Link_Edit, participantId]);
  }

  openParticipantCreating(taskId: string) {
    this.router.navigate([RoutingNodes.Participant, RoutingNodes.Link_Create, this.itemId, taskId]);
  }

  openReplyForParticipant(taskId: string) {
    this.router.navigate([RoutingNodes.TasksOnWorkLocation, this.itemId, RoutingNodes.ReplyForParticipant, taskId]);
  }

  openAssignParticipantDialog(task: UpdatingTaskAssignment, teamIndex: number) {
    this.assigningTask = task;
    this.selectedTeamIndex = teamIndex;
    this.form.value.selectedParticipantIds = [];
    this.openAddParticipant = true;
  }

  onParticipantPickerClose() {
    this.openAddParticipant = false;
  }

  onParticipantSelected() {
    this.assigningTask.isUpdating = true;

    const request: AssignParticipantToTaskRequest = {
      electionId: this.assigningTask.electionId,
      taskAssignmentId: this.assigningTask.id,
      participantId: this.form.value.selectedParticipantIds[0],
      taskTypeId: this.assigningTask.taskTypeId
    };

    this.subs.sink = this.workLocationTasksHttpService.assignParticipantToTask(request).subscribe((res) => {
      if (res.isSuccess) {
        this.loading = true;
        this.workLocationTasksHttpService.getTeamTasks(this.dailyTeamTasks[this.selectedTeamIndex].teamId, this.itemId, this.election.id, false).subscribe((teamTasksResult) => {
          this.dailyTeamTasks[this.selectedTeamIndex].tasks = teamTasksResult.data;
          this.dailyTeamTasks[this.selectedTeamIndex].displayingTasks = teamTasksResult.data;
        });

        this.subs.sink = this.workLocationTasksHttpService.getWorkLocationTasksSummary(this.itemId, this.election.id).subscribe((taskTypesSummaryResult) => {
          this.workLocationTasksSummary = taskTypesSummaryResult.data;
          this.buildWorkLocationTasksSummary();
        });

        this.assigningTask.isUpdating = false;
      } else {
        this.assigningTask.isUpdating = false;
      }
    });
  }

  copyTaskLink(taskDetailsPageUrl: string) {
    this.clipboard.copy(taskDetailsPageUrl);
    this.notificationService.showSuccess(this.translocoService.translate('tasks.success.task_link_copied'));
  }

  removeParticipant(task: UpdatingTaskAssignment, teamIndex) {
    this.selectedTeamIndex = teamIndex;
    this.subs.sink = this.subs.sink = this.dialog
      .open(ConfirmationDialogComponent, {
        minWidth: 400,
        data: {
          title: "tasks.actions.remove_participant",
          content: 'tasks.actions.remove_participant_confirmation',
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result === true) {
          task.isUpdating = true;
          const request: RemoveParticipantFromTaskRequest = {
            electionId: task.electionId,
            taskAssignmentId: task.id
          };

          this.subs.sink = this.workLocationTasksHttpService.removeParticipantFromTask(request).subscribe((res) => {
            if (res.isSuccess) {
              this.loading = true;

              this.workLocationTasksHttpService.getTeamTasks(this.dailyTeamTasks[this.selectedTeamIndex].teamId, this.itemId, this.election.id, false).subscribe((teamTasksResult) => {
                this.dailyTeamTasks[this.selectedTeamIndex].tasks = teamTasksResult.data;
                this.dailyTeamTasks[this.selectedTeamIndex].displayingTasks = teamTasksResult.data;
              });

              this.subs.sink = this.workLocationTasksHttpService.getWorkLocationTasksSummary(this.itemId, this.election.id).subscribe((taskTypesSummaryResult) => {
                this.workLocationTasksSummary = taskTypesSummaryResult.data;
                this.buildWorkLocationTasksSummary();
              });

              task.isUpdating = false;
            } else {
              task.isUpdating = false;
            }
          });
        }
      });
  }
  moveVacantTasksToOtherTeam(currentTeamId: string, teamIndex: number) {
    const tasksIds = this.dailyTeamTasks[teamIndex].tasks
      .filter(t => t.teamId == currentTeamId && t.participantId == null)
      .map(t => t.id);

    const request: MoveTasksRequest = {
      taskIds: tasksIds,
      targetTeamId: this.selectedTeam as unknown as string,
      sourceTeamId: currentTeamId
    };
    this.workLocationTasksHttpService.moveTasks(request).subscribe((res) => {

      this.workLocationTasksHttpService.getTeamTasks(this.dailyTeamTasks[teamIndex].teamId, this.itemId, this.election.id, false).subscribe((teamTasksResult) => {
        this.dailyTeamTasks[teamIndex].tasks = teamTasksResult.data;
        this.dailyTeamTasks[teamIndex].displayingTasks = teamTasksResult.data;
      });
      this.subs.sink = this.workLocationTasksHttpService.getWorkLocationTasksSummary(this.itemId, this.election.id).subscribe((taskTypesSummaryResult) => {
        this.workLocationTasksSummary = taskTypesSummaryResult.data;
        this.buildWorkLocationTasksSummary();
        this.buildTeamTasksSummary();
        this.buildRejectedTeamTasksSummary();
      });
      this.dailyTeamTasks[teamIndex].loadingTasks = false;

    });
  }

  onTeamChanged() {
    this.targetTeamId = this.selectedTeam;
  }
}

