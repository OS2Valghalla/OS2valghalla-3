import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { combineLatest } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { SubSink } from 'subsink';
import { FormBuilder } from '@angular/forms';
import { TranslocoService } from '@ngneat/transloco';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { GlobalStateService } from 'src/app/global-state.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { WorkLocationTasksHttpService } from '../../services/work-location-tasks-http.service';
import { TaskAssignment } from '../../models/task-assignment';
import { ReplyForParticipantRequest } from '../../models/reply-for-participant-request';
import { ParticipantDetails } from '../../../participant/models/participant-details';
import { ParticipantHttpService } from '../../../participant/services/participant-http.service';
import { SpecialDietSharedHttpService } from 'src/shared/services/special-diet-shared-http.service';
import { SpecialDietShared } from 'src/shared/models/special-diet/special-diet-shared';

@Component({
  selector: 'app-tasks-reply-for-participant',
  templateUrl: './reply-for-participant.component.html',
  providers: [WorkLocationTasksHttpService, ParticipantHttpService],
})
export class ReplyForParticipantComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  
  workLocationId: string;

  taskAssignmentId: string;

  election?: ElectionShared;

  task: TaskAssignment;

  selectedResponse: boolean;

  loadingParticipantDetails = true;

  participantDetails: ParticipantDetails;

  specialDiets: SpecialDietShared[] = [];  

  readonly form = this.formBuilder.group({
    mobileNumber: [''],
    email: [''],
    specialDietIds: [[] as string[]]
  });

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private globalStateService: GlobalStateService,
    private breadcrumbService: BreadcrumbService,
    private translocoService: TranslocoService,
    private formBuilder: FormBuilder,
    private workLocationTasksHttpService: WorkLocationTasksHttpService,
    private participantHttpService: ParticipantHttpService,
    private specialDietSharedHttpService: SpecialDietSharedHttpService,
    ) {}

  ngOnInit(): void {
    this.workLocationId = this.route.snapshot.paramMap.get(RoutingNodes.WorkLocationId);
    this.taskAssignmentId = this.route.snapshot.paramMap.get(RoutingNodes.TaskId);
    this.subs.sink = this.globalStateService.election$.subscribe((election) => {
        this.election = election;
        this.subs.sink = this.workLocationTasksHttpService.getTaskAssignment(this.taskAssignmentId, this.election.id).subscribe((res) => {
          this.task = res.data;
          this.breadcrumbService.set('/' + RoutingNodes.TasksOnWorkLocation + '/:' + RoutingNodes.WorkLocationId, this.translocoService.translate('tasks.work_location_tasks.tasks_on') + ' ' + this.task.workLocationName);
          this.loading = false;
        });
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onSelectedResponseChanged(event) {
    this.form.markAsDirty();
    if (this.selectedResponse && !this.participantDetails) {
      this.subs.sink = combineLatest({
        specialDiets: this.specialDietSharedHttpService.getSpecialDiets(),
        participantDetails: this.participantHttpService.getParticipantDetails(this.task.participantId),
      }).subscribe((d) => {
        this.specialDiets = d.specialDiets.data;
        this.participantDetails = d.participantDetails.data;
        this.loadingParticipantDetails = false;
        this.form.controls.mobileNumber.setValue(this.participantDetails.mobileNumber);
        this.form.controls.email.setValue(this.participantDetails.email);
        this.form.controls.specialDietIds.setValue(this.participantDetails.specialDietIds);        
      });
    }
  }

  replyForParticipant(event: FormPageEvent) {
    const request: ReplyForParticipantRequest = {
      electionId: this.election.id,
      taskAssignmentId: this.taskAssignmentId,
      accepted: this.selectedResponse,
      mobileNumber: this.form.value.mobileNumber,
      email: this.form.value.email,
      specialDietIds: this.form.value.specialDietIds,
    };

    this.subs.sink = event.pipe(this.workLocationTasksHttpService.replyForParticipant(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.router.navigate([RoutingNodes.TasksOnWorkLocation, this.workLocationId]);
      }
    });
  }

  onCancel() {
    this.router.navigate([RoutingNodes.TasksOnWorkLocation, this.workLocationId]);
  }
}
