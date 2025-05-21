import { Component } from '@angular/core';
import { SubSink } from 'subsink';
import { TaskPreview } from '../../models/task-preview';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskHttpService } from '../../services/task-http.service';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { finalize } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-task-details',
  templateUrl: './task-details.component.html',
  providers: [TaskHttpService],
})
export class TaskDetailsComponent {
  private readonly subs = new SubSink();

  loading: boolean = true;
  taskPreview: TaskPreview;
  signedIn: boolean = false;

  private hashValue: string;

  constructor(
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
    private readonly taskHttpService: TaskHttpService,
  ) { }

  ngOnInit(): void {
    this.hashValue = this.route.snapshot.paramMap.get(RoutingNodes.Id);

    if (!this.hashValue) {
      this.router.navigate(['/']);
    }

    this.signedIn = this.authService.isSessionExist();

    this.subs.sink = this.taskHttpService
      .getTaskPreview(this.hashValue)
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

  register() {
    this.router.navigate([RoutingNodes.Registration, RoutingNodes.TaskRegistration, this.hashValue]);
  }
  rejectTask() {
    this.taskHttpService.rejectTask(this.hashValue).subscribe((res) => {
      if (res.isSuccess && res.data.succeed) {
        this.router.navigate([RoutingNodes.MyTasks]);
      }
    }
    );
  }
}
