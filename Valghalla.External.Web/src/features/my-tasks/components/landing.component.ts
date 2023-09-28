import { Component, AfterViewInit } from '@angular/core';
import { SubSink } from 'subsink';
import { TranslocoService } from '@ngneat/transloco';
import { TaskDetails } from '../../tasks/models/task-preview';
import { TaskHttpService } from '../../tasks/services/task-http.service';
import { finalize } from 'rxjs';
import { AppWeekdayNamePipe } from 'src/shared/pipes/weekday-name.pipe';
import { AppMonthNamePipe } from 'src/shared/pipes/month-name.pipe';
import { FileReference } from 'src/shared/models/file-reference';
import { GlobalStateService } from 'src/app/global-state.service';

@Component({
  selector: 'app-my-tasks-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
  providers: [AppWeekdayNamePipe, AppMonthNamePipe, TaskHttpService],
})
export class MyTasksLandingComponent implements AfterViewInit{
  private readonly subs = new SubSink();

  loading = true;

  tasks: Array<TaskDetails> = [];
  
  constructor(
    private readonly globalStateService: GlobalStateService,
    private readonly appWeekdayNamePipe: AppWeekdayNamePipe,
    private readonly appMonthNamePipe: AppMonthNamePipe,
    private readonly translocoService: TranslocoService,
    private readonly taskHttpService: TaskHttpService,
  ) {}

  ngAfterViewInit () {
    this.subs.sink = this.taskHttpService
    .getMyTasks()
    .pipe(
      finalize(() => {
        this.loading = false;
      }),
    )
    .subscribe((res) => {
      if (res.isSuccess) {
        this.tasks = res.data;
      }
    });
  }

  unregisterTask(taskAssignmentId) {
    this.taskHttpService.unregisterTask(taskAssignmentId).subscribe((res) => {
      if (res.isSuccess && res.data.succeed) {
        var deletedTask = this.tasks.filter(f => f.taskAssignmentId == taskAssignmentId)[0];
        this.tasks.splice(this.tasks.indexOf(deletedTask), 1);

        this.globalStateService.userTeamFetching.next();
      }
    });
  } 

  getTaskDateFriendlyText(taskDate) {
    var date = new Date(taskDate);
    return this.translocoService.translate(this.appWeekdayNamePipe.transform(date)) + ' ' + date.getDate() + '. ' + this.translocoService.translate(this.appMonthNamePipe.transform(date)) + ' ' + date.getFullYear();
  }

  getFileDownloadLink(file: FileReference) {
    return this.taskHttpService.getDownloadFileLink(file.id);
  }
}
