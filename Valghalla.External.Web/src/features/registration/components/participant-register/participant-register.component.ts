import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, Validators } from '@angular/forms';
import { combineLatest, finalize } from 'rxjs';
import { SpecialDietShared } from 'src/shared/models/special-diet/special-diet-shared';
import { WebPage } from 'src/shared/models/web-page';
import { SpecialDietSharedHttpService } from 'src/shared/services/special-diet-shared-http.service';
import { WebHttpService } from 'src/shared/services/web-http.service';
import { SubSink } from 'subsink';
import { ParticipantRegisterRequest } from '../../models/participant-register-request';
import { ActivatedRoute } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { RegistrationHttpService } from '../../services/registration-http.service';
import { emailValidator } from 'src/shared/validators/email-validator';

@Component({
  selector: 'app-participant-register',
  templateUrl: './participant-register.component.html',
  providers: [RegistrationHttpService],
})
export class ParticipantRegisterComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  @Input() type: 'team' | 'task';

  @Output() next = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  loading: boolean = true;
  saving: boolean = false;
  specialDiets: SpecialDietShared[] = [];
  page?: WebPage;

  private hashValue: string;
  private invitationCode?: string;

  readonly form = this.formBuilder.group({
    mobileNumber: [undefined as string, Validators.required],
    email: [undefined as string, [Validators.required, emailValidator()]],
    specialDietIds: [[] as string[]],
    consentApproved: [false],
  });

  constructor(
    private readonly route: ActivatedRoute,
    private readonly formBuilder: FormBuilder,
    private readonly specialDietSharedHttpService: SpecialDietSharedHttpService,
    private readonly webHttpService: WebHttpService,
    private readonly registrationHttpService: RegistrationHttpService,
  ) {}

  ngOnInit(): void {
    this.hashValue = this.route.snapshot.paramMap.get(RoutingNodes.Id);
    this.invitationCode = this.route.snapshot.queryParamMap.get(RoutingNodes.TaskInvitationCode);

    this.subs.sink = combineLatest({
      specialDiets: this.specialDietSharedHttpService.getSpecialDiets(),
      page: this.webHttpService.getDeclarationOfConsentPage(),
    }).subscribe((d) => {
      this.loading = false;
      this.specialDiets = d.specialDiets.data;
      this.page = d.page.data;
      this.form.controls.consentApproved.setValue(!this.page.isActivated);
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  onSpecialDietsChanged(event: Event) {
    const specialDietId = (event.target as any).value;

    if (this.form.value.specialDietIds.includes(specialDietId)) {
      this.form.controls.specialDietIds.setValue(this.form.value.specialDietIds.filter((id) => id != specialDietId));
    } else {
      this.form.controls.specialDietIds.setValue(this.form.value.specialDietIds.concat(specialDietId));
    }
  }

  register() {
    if (!this.form.valid) return;

    this.saving = true;
    this.form.disable();

    const request: ParticipantRegisterRequest = {
      hashValue: this.hashValue,
      mobileNumber: this.form.value.mobileNumber,
      email: this.form.value.email,
      specialDietIds: this.form.value.specialDietIds,
    };

    if (this.type == 'team') {
      this.subs.sink = this.registrationHttpService
        .registerParticipantWithTeam(request)
        .pipe(
          finalize(() => {
            this.saving = false;
            this.form.enable();
          }),
        )
        .subscribe((res) => {
          if (res.isSuccess) {
            this.next.emit();
          }
        });
    } else if (this.type == 'task') {
      this.subs.sink = this.registrationHttpService
        .registerParticipantWithTask(request, this.invitationCode)
        .pipe(
          finalize(() => {
            this.saving = false;
            this.form.enable();
          }),
        )
        .subscribe((res) => {
          if (res.isSuccess) {
            this.next.emit();
          }
        });
    }
  }

  cancelRegistration() {
    this.cancel.emit();
  }
}
