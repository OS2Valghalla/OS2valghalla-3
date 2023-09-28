import { Component } from '@angular/core';
import { SubSink } from 'subsink';
import { TranslocoService } from '@ngneat/transloco';
import { TaskPreview } from '../../models/task-preview';
import { ActivatedRoute, Router } from '@angular/router';
import { Clipboard } from '@angular/cdk/clipboard';
import { NotificationService } from 'src/shared/services/notification.service';
import { TeamResponsibleTaskHttpService } from '../../services/team-responsible-task-http.service';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { finalize } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-task-distribution-details',
  styleUrls: ['./task-details.component.scss'],
  templateUrl: './task-details.component.html',
  providers: [TeamResponsibleTaskHttpService],
})
export class TaskDistributionDetailsComponent {
  private readonly subs = new SubSink();

  loading: boolean = true;
  taskPreview: TaskPreview;
  signedIn: boolean = false;
  taskLink: string;

  private hashValue: string;

  constructor(
    private readonly clipboard: Clipboard,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
    private readonly taskHttpService: TeamResponsibleTaskHttpService,
  ) {}

  ngOnInit(): void {
    this.hashValue = this.route.snapshot.paramMap.get(RoutingNodes.Id);

    if (!this.hashValue) {
      this.router.navigate(['/']);
    }

    this.signedIn = this.authService.isSessionExist();    

    this.subs.sink = this.taskHttpService
      .getTeamResponsibleTaskPreview(this.hashValue)
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe((res) => {
        if (res.isSuccess) {
          this.taskPreview = res.data;          
        }
      });
  }

  back() {
    history.back();
  }

  copyLinkToTask() {
    this.taskLink = window.location.origin + '/' + RoutingNodes.Tasks + '/' + RoutingNodes.TaskDetails + '/' + this.hashValue;
    this.clipboard.copy(this.taskLink);
    this.notificationService.showSuccess(this.translocoService.translate('tasks.labels.task_link_copied'));
  }

  register() {
    this.router.navigate([RoutingNodes.Registration, RoutingNodes.TaskRegistration, this.hashValue]);
  }
}
