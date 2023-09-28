import { Component, OnDestroy, OnInit } from '@angular/core';
import { TaskPreview } from '../../models/task-preview';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TaskHttpService } from '../../services/task-http.service';
import { SubSink } from 'subsink';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-task-invitation',
  templateUrl: './task-invitation.component.html',
  providers: [TaskHttpService],
})
export class TaskInvitationComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  private hashValue: string;
  private invitationCode: string;

  loading: boolean = true;
  taskPreview: TaskPreview;

  constructor(
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly taskHttpService: TaskHttpService,
  ) {}

  ngOnInit(): void {
    this.hashValue = this.route.snapshot.paramMap.get(RoutingNodes.Id);
    this.invitationCode = this.route.snapshot.paramMap.get(RoutingNodes.TaskInvitationCode);

    if (!this.hashValue || !this.invitationCode) {
      this.router.navigate(['/']);
    }

    this.subs.sink = this.taskHttpService
      .getTaskPreview(this.hashValue, this.invitationCode)
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

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  acceptTask() {    
    this.router.navigate([RoutingNodes.Registration, RoutingNodes.TaskRegistration, this.hashValue], {
      queryParams: {
        [RoutingNodes.TaskInvitationCode]: this.invitationCode,
      },
    });
  }

  rejectTask() {
    this.router.navigate([RoutingNodes.Registration, RoutingNodes.TaskRegistration, RoutingNodes.Cancellation, this.hashValue], {
      queryParams: {
        [RoutingNodes.TaskInvitationCode]: this.invitationCode,
      },
    });
  }
}
