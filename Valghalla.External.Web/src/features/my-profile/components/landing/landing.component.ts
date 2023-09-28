import { Component, OnDestroy, OnInit } from '@angular/core';
import { MyProfile } from '../../models/my-profile';
import { MyProfileHttpService } from '../../services/my-profile-http.service';
import { SubSink } from 'subsink';
import { FormBuilder, Validators } from '@angular/forms';
import { combineLatest, finalize } from 'rxjs';
import { SpecialDietSharedHttpService } from 'src/shared/services/special-diet-shared-http.service';
import { SpecialDietShared } from 'src/shared/models/special-diet/special-diet-shared';
import { UpdateMyProfileRequest } from '../../models/update-my-profile-request';
import { MyProfilePermission } from '../../models/my-profile-permission';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { emailValidator } from 'src/shared/validators/email-validator';

@Component({
  selector: 'app-my-profile-landing',
  templateUrl: './landing.component.html',
  providers: [MyProfileHttpService],
})
export class MyProfileLandingComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  readonly modalId = crypto.randomUUID();
  readonly modalHeadingId = crypto.randomUUID();
  readonly contactUsUrl: string = '/' + RoutingNodes.ContactUs;

  loading: boolean = true;
  saving: boolean = false;
  deleting: boolean = false;
  specialDiets: SpecialDietShared[] = [];
  profile?: MyProfile;
  permission?: MyProfilePermission;

  readonly form = this.formBuilder.group({
    mobileNumber: [undefined as string, Validators.required],
    email: [undefined as string, [Validators.required, emailValidator()]],
    specialDietIds: [[] as string[]],
  });

  constructor(
    private readonly router: Router,
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly myProfileHttpService: MyProfileHttpService,
    private readonly specialDietSharedHttpService: SpecialDietSharedHttpService,
  ) {}

  ngOnInit(): void {
    this.subs.sink = combineLatest({
      specialDiets: this.specialDietSharedHttpService.getSpecialDiets(),
      profile: this.myProfileHttpService.getMyProfile(),
      permission: this.myProfileHttpService.getMyProfilePermission(),
    }).subscribe((d) => {
      this.loading = false;
      this.specialDiets = d.specialDiets.data;
      this.permission = d.permission.data;

      if (d.profile.isSuccess) {
        this.profile = d.profile.data;
        this.form.setValue({
          mobileNumber: d.profile.data.mobileNumber,
          email: d.profile.data.email,
          specialDietIds: d.profile.data.specialDietIds,
        });
        this.form.controls.mobileNumber.markAsDirty();
        this.form.controls.email.markAsDirty();
      }
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

  save() {
    if (!this.form.valid) return;

    this.saving = true;
    this.form.disable();

    const request: UpdateMyProfileRequest = {
      mobileNumber: this.form.value.mobileNumber,
      email: this.form.value.email,
      specialDietIds: this.form.value.specialDietIds,
    };

    this.subs.sink = this.myProfileHttpService
      .updateMyProfile(request)
      .pipe(
        finalize(() => {
          this.saving = false;
          this.form.enable();
        }),
      )
      .subscribe();
  }

  delete() {
    this.deleting = true;
    this.form.disable();

    this.subs.sink = this.myProfileHttpService
      .deleteMyProfile()
      .pipe(
        finalize(() => {
          this.deleting = false;
          this.form.enable();
        }),
      )
      .subscribe((res) => {
        if (res.isSuccess) {
          this.authService.logout(true);
        }
      });
  }
}
