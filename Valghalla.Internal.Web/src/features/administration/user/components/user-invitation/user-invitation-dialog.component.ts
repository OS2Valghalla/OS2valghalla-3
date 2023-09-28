import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { SubSink } from 'subsink';
import { FormDialogEvent } from '../../../../../shared/models/ux/formDialog';
import { InviteUserRequest } from '../../models/invite-user-request';
import { UserRole } from '../../models/user-role';
import { UserHttpService } from '../../services/user-http.service';

@Component({
  selector: 'app-user-invitation-dialog',
  templateUrl: './user-invitation-dialog.component.html',
  providers: [UserHttpService],
})
export class UserInvitationDialogComponent implements OnInit, OnDestroy {
  private subs = new SubSink();

  loading = true;

  roles: UserRole[];

  constructor(private formBuilder: FormBuilder, private userHttpService: UserHttpService) {}

  readonly form = this.formBuilder.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]],
    roleId: ['', Validators.required],
  });

  ngOnInit(): void {
    this.subs.sink = this.userHttpService.getUserRoles().subscribe((res) => {
      this.loading = false;

      if (res.isSuccess) {
        this.roles = res.data;
      }
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  submit(formEvent: FormDialogEvent) {
    const request: InviteUserRequest = {
      name: this.form.value.name,
      email: this.form.value.email,
      roleId: this.form.value.roleId,
    };

    formEvent.pipe(this.userHttpService.inviteUser(request), (observable, handler) => {
      this.subs.sink = observable.subscribe((res) => {
        if (res.isSuccess) {
          handler.next('administration.success.user.invite_user');
        } else {
          handler.error(res.errors.validation);
        }
      });
    });
  }
}
