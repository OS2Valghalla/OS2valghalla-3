import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, ViewChild, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { GlobalStateService } from 'src/app/global-state.service';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { ParticipantHttpService } from '../../services/participant-http.service';
import { ParticipantDetails } from '../../models/participant-details';
import { CreateParticipantRequest } from '../../models/create-participant-request';
import { TeamSharedHttpService } from 'src/shared/services/team-shared-http.service';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { catchError, combineLatest, throwError } from 'rxjs';
import { SpecialDietSharedHttpService } from 'src/shared/services/special-diet-shared-http.service';
import { SpecialDietShared } from 'src/shared/models/special-diet/special-diet-shared';
import { UpdateParticipantRequest } from '../../models/update-participant-request';
import { emailValidator } from 'src/shared/validators/email-validator';

@Component({
  selector: 'app-participant-item',
  templateUrl: './participant-item.component.html',
  providers: [ParticipantHttpService],
})
export class ParticipantItemComponent implements AfterViewInit, OnDestroy, OnInit {
  private readonly subs = new SubSink();

  loading: boolean = true;
  cprChecking: boolean = false;
  item?: ParticipantDetails;

  teams: TeamShared[] = [];
  specialDiets: SpecialDietShared[] = [];

  electionId: string;

  workLocationId? : string;
  taskId? : string;

  readonly form = this.formBuilder.group({
    cpr: ['', Validators.required],
    mobileNumber: [''],
    email: ['', emailValidator()],
    specialDietIds: [undefined as string[]],
    memberTeamIds: [undefined as string[], Validators.required],
    cprVerified: [false, Validators.requiredTrue],
  });

  readonly recordForm = this.formBuilder.group({
    firstName: [''],
    lastName: [''],
    streetAddress: [''],
    postalCode: [''],
    city: [''],
    coAddress: [''],
    birthdate: [undefined as Date],
    deceased: [false],
    disenfranchised: [false],
    exemptDigitalPost: [false],
  });

  get cprCheckEnabled(): boolean {
    return !this.form.controls.cpr.disabled && this.form.value.cpr.length > 0;
  }

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private readonly formBuilder: FormBuilder,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private globalStateService: GlobalStateService,
    private readonly teamSharedHttpService: TeamSharedHttpService,
    private readonly specialDietSharedHttpService: SpecialDietSharedHttpService,
    private readonly participantHttpService: ParticipantHttpService,
  ) {}

  ngOnInit(): void {
    this.workLocationId = this.route.snapshot.paramMap.get(RoutingNodes.WorkLocationId);
    this.taskId = this.route.snapshot.paramMap.get(RoutingNodes.TaskId);
    this.subs.sink = this.globalStateService.election$.subscribe((election) => {
      this.electionId = election.id;
    });
  }

  ngAfterViewInit(): void {
    this.recordForm.disable();

    this.subs.sink = combineLatest({
      teams: this.teamSharedHttpService.getTeams(),
      specialDiets: this.specialDietSharedHttpService.getSpecialDiets(),
      state: this.formPage.state$,
    }).subscribe((d) => {
      this.teams = d.teams.data;
      this.specialDiets = d.specialDiets.data;

      if (!this.formPage.isUpdateForm()) {
        this.loading = false;
        this.changeDetectorRef.detectChanges();
        return;
      }

      this.subs.sink = this.participantHttpService.getParticipantDetails(this.formPage.itemId).subscribe((res) => {
        this.loading = false;
        this.item = res.data;

        if (res.data) {
          const value = res.data;

          this.form.setValue({
            cpr: value.cpr,
            mobileNumber: value.mobileNumber,
            email: value.email,
            specialDietIds: value.specialDietIds,
            memberTeamIds: value.memberTeamIds,
            cprVerified: true,
          });

          this.recordForm.setValue({
            firstName: value.firstName,
            lastName: value.lastName,
            streetAddress: value.streetAddress,
            postalCode: value.postalCode,
            city: value.city,
            coAddress: value.coAddress,
            birthdate: value.birthdate,
            deceased: value.deceased,
            disenfranchised: value.disenfranchised,
            exemptDigitalPost: value.exemptDigitalPost,
          });

          this.form.controls.cpr.disable();
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  createParticipant(event: FormPageEvent) {
    const request: CreateParticipantRequest = {
      cpr: this.form.value.cpr,
      mobileNumber: this.form.value.mobileNumber,
      email: this.form.value.email,
      specialDietIds: this.form.value.specialDietIds,
      teamIds: this.form.value.memberTeamIds,
    };

    if (this.workLocationId && this.taskId) {
      request.taskId = this.taskId;
      request.electionId = this.electionId;
    }

    this.subs.sink = event.pipe(this.participantHttpService.createParticipant(request)).subscribe((res) => {
      if (res.isSuccess) {
        if (this.workLocationId && this.taskId) {          
          this.router.navigate([RoutingNodes.TasksOnWorkLocation, this.workLocationId]);
        }
        else if (res.data) {
          this.formPage.navigate([RoutingNodes.Participant, RoutingNodes.Profile, res.data]);
        }
        else {
          this.formPage.navigate();
        }
      }
    });
  }

  updateParticipant(event: FormPageEvent) {
    const request: UpdateParticipantRequest = {
      id: this.formPage.itemId,
      mobileNumber: this.form.value.mobileNumber,
      email: this.form.value.email,
      specialDietIds: this.form.value.specialDietIds,
      teamIds: this.form.value.memberTeamIds,
    };

    this.subs.sink = event.pipe(this.participantHttpService.updateParticipant(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  deleteParticipant(event: FormPageEvent) {
    this.subs.sink = event
      .pipe(this.participantHttpService.deleteParticipant(this.formPage.itemId))
      .subscribe((res) => {
        if (res.isSuccess) {
          this.formPage.navigate();
        }
      });
  }

  cancel() {
    if (this.workLocationId && this.taskId) {
      this.router.navigate([RoutingNodes.TasksOnWorkLocation, this.workLocationId]);
    }
    else {
      this.formPage.navigate();
    }
  }

  checkCprNumber() {
    this.cprChecking = true;

    this.subs.sink = this.participantHttpService
      .getParticipantPersonalRecord(this.form.value.cpr)
      .pipe(
        catchError((error) => {
          this.cprChecking = false;
          return throwError(() => error);
        }),
      )
      .subscribe((res) => {
        this.cprChecking = false;

        if (res.isSuccess) {
          const value = res.data;
          this.recordForm.setValue({
            firstName: value.firstName,
            lastName: value.lastName,
            streetAddress: value.streetAddress,
            postalCode: value.postalCode,
            city: value.city,
            coAddress: value.coAddress,
            birthdate: value.birthdate,
            deceased: value.deceased,
            disenfranchised: value.disenfranchised,
            exemptDigitalPost: value.exemptDigitalPost,
          });

          this.form.controls.cprVerified.setValue(true);
        }
      });
  }
}
