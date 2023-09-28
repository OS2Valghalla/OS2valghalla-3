import { Component, OnDestroy, OnInit } from '@angular/core';
import { TaskPreview } from '../../models/task-preview';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TaskHttpService } from '../../services/task-http.service';
import { SubSink } from 'subsink';
import { finalize } from 'rxjs';
import { GlobalStateService } from 'src/app/global-state.service';

@Component({
  selector: 'app-task-acceptance-confirmation',
  templateUrl: './task-acceptance-confirmation.component.html',
  providers: [TaskHttpService],
})
export class TaskAcceptanceConfirmationComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  loading: boolean = true;
  stepIndicatorVisible: boolean = false;
  taskPreview: TaskPreview;

  constructor(
    private readonly globalStateService: GlobalStateService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly taskHttpService: TaskHttpService,
  ) {}

  ngOnInit(): void {
    const hashValue = this.route.snapshot.paramMap.get(RoutingNodes.Id);

    if (!hashValue) {
      this.router.navigate(['/']);
    }

    this.stepIndicatorVisible = this.route.snapshot.queryParamMap.get('stepper') == 'true';

    this.globalStateService.userTeamFetching.next();

    this.subs.sink = this.taskHttpService
      .getTaskPreview(hashValue)
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
}
