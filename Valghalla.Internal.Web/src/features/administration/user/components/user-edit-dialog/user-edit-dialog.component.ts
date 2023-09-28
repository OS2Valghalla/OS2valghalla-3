import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { FormDialogEvent } from '../../../../../shared/models/ux/formDialog';
import { FormBuilder, Validators } from '@angular/forms';
import { UserHttpService } from '../../services/user-http.service';
import { UserRole } from '../../models/user-role';
import { UpdateUserRequest } from '../../models/update-user-request';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GetAllUsersItem } from '../../models/get-all-users-response';

@Component({
  selector: 'app-user-edit-dialog',
  templateUrl: './user-edit-dialog.component.html',
  providers: [UserHttpService],
})
export class UserEditDialogComponent implements OnInit, OnDestroy {
  private subs = new SubSink();

  loading = true;

  roles: UserRole[];

  constructor(
    private formBuilder: FormBuilder,
    private userHttpService: UserHttpService,
    @Inject(MAT_DIALOG_DATA)
    private dialogData: {
      editingItem: GetAllUsersItem;
    },
  ) {}

  readonly form = this.formBuilder.group({
    name: ['', Validators.required],
    roleId: ['', Validators.required],
  });

  ngOnInit(): void {
    if (this.dialogData && this.dialogData.editingItem && this.dialogData.editingItem.id) {
      this.form.setValue({
        name: this.dialogData.editingItem.name,
        roleId: this.dialogData.editingItem.roleId,
      });
      this.form.markAllAsTouched();
    }
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
    const request: UpdateUserRequest = {
      id: this.dialogData.editingItem.id,
      name: this.form.value.name,
      roleId: this.form.value.roleId,
    };

    formEvent.pipe(this.userHttpService.updateUser(request), (observable, handler) => {
      this.subs.sink = observable.subscribe((res) => {
        if (res.isSuccess) {
          handler.next('administration.success.user.update_user');
        } else {
          handler.error(res.errors.validation);
        }
      });
    });
  }
}
