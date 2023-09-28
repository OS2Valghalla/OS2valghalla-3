import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { SubSink } from 'subsink';
import { RegistrationHttpService } from '../../services/registration-http.service';
import { SpecialDietSharedHttpService } from 'src/shared/services/special-diet-shared-http.service';
import { WebHttpService } from 'src/shared/services/web-http.service';
import { combineLatest, finalize } from 'rxjs';
import { MyProfileRegistration } from '../../models/my-profile-registration';
import { SpecialDietShared } from 'src/shared/models/special-diet/special-diet-shared';
import { WebPage } from 'src/shared/models/web-page';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TaskHttpService } from 'src/features/tasks/services/task-http.service';

@Component({
  selector: 'app-my-profile-registration-confirmator',
  templateUrl: './my-profile-registration-confirmator.component.html',
  providers: [RegistrationHttpService, TaskHttpService],
})
export class MyProfileRegistrationConfirmatorComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  @Input() type: 'team' | 'task';
  @Input() linkVisible: boolean;

  @Output() next = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  saving: boolean = false;
  loading: boolean = true;
  profile?: MyProfileRegistration;
  specialDiets: SpecialDietShared[] = [];
  page?: WebPage;

  private hashValue: string;
  private invitationCode?: string;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly registrationHttpService: RegistrationHttpService,
    private readonly specialDietSharedHttpService: SpecialDietSharedHttpService,
    private readonly webHttpService: WebHttpService,
    private readonly taskHttpService: TaskHttpService,
  ) {}

  ngOnInit(): void {
    this.hashValue = this.route.snapshot.paramMap.get(RoutingNodes.Id);
    this.invitationCode = this.route.snapshot.queryParamMap.get(RoutingNodes.TaskInvitationCode);

    this.subs.sink = combineLatest({
      profile: this.registrationHttpService.getMyProfileRegistration(),
      specialDiets: this.specialDietSharedHttpService.getSpecialDiets(),
      page: this.webHttpService.getDeclarationOfConsentPage(),
    }).subscribe((d) => {
      this.loading = false;
      this.profile = d.profile.data;
      this.specialDiets = d.specialDiets.data;
      this.page = d.page.data;
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  continue() {
    if (this.type == 'team') {
      const redirectUrl = [window.location.origin, RoutingNodes.Tasks].join('/');
      window.location.href = redirectUrl;
    } else if (this.type == 'task') {
      this.saving = true;
      this.subs.sink = this.taskHttpService
        .acceptTask(this.hashValue, this.invitationCode, true)
        .pipe(
          finalize(() => {
            this.saving = false;
          }),
        )
        .subscribe();
    }
  }

  goToTasks() {
    this.router.navigate([RoutingNodes.Tasks]);
  }

  cancelRegistration() {
    this.cancel.emit();
  }

  renderSpecialDietNames(specialDietIds: string[]) {
    return specialDietIds.map((id) => this.specialDiets.find((i) => i.id == id)?.title).join(', ');
  }
}
