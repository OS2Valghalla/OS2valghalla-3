import { Component, ViewChild } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { combineLatest } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/shared/components/confirmation-dialog/confirmation-dialog.component';
import { TableComponent } from 'src/shared/components/table/table.component';
import { PageMenuItem } from 'src/shared/models/ux/PageMenuItem';
import { NotificationService } from 'src/shared/services/notification.service';
import { SubSink } from 'subsink';
import { Response } from '../../../shared/models/respone';
import { TableEditRowEvent } from '../../../shared/models/ux/table';
import { UserEditDialogComponent } from './components/user-edit-dialog/user-edit-dialog.component';
import { UserInvitationDialogComponent } from './components/user-invitation/user-invitation-dialog.component';
import { GetAllUsersItem } from './models/get-all-users-response';
import { UserRole } from './models/user-role';
import { UserType } from './models/user-type';
import { UserHttpService } from './services/user-http.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  providers: [UserHttpService],
})
export class UserComponent {
  private subs = new SubSink();
  loading = true;
  userRoles: UserRole[] = [];
  users: GetAllUsersItem[] = [];

  readonly menuItems: PageMenuItem[] = [
    {
      title: 'administration.user.invite_user',
      matIcon: 'add',
      onSelectItem: () => this.openUserInvitationDialog(),
    },
  ];

  @ViewChild('tableUsers')
  private readonly tableUsers: TableComponent<GetAllUsersItem>;

  constructor(
    private readonly dialog: MatDialog,
    private readonly userHttpService: UserHttpService,
    private readonly translocoService: TranslocoService,
    private readonly notificationService: NotificationService,
  ) {}

  openUserInvitationDialog() {
    this.dialog
      .open(UserInvitationDialogComponent)
      .afterClosed()
      .subscribe((result) => {
        if (result == 'administration.success.user.invite_user') {
          this.tableUsers.refresh();
        }
      });
  }

  getRoleName(roleId: string) {
    return this.userRoles.find((i) => i.id == roleId)?.name;
  }

  onLoadEvent() {
    this.loading = true;

    this.subs.sink = combineLatest({
      userRoles: this.userHttpService.getUserRoles(),
      users: this.userHttpService.getAllUsers(),
    }).subscribe(({ userRoles, users }) => {
      this.loading = false;

      if (userRoles.isSuccess) {
        this.userRoles = userRoles.data;
      }

      if (users.isSuccess) {
        this.users = users.data.items;
      }
    });
  }

  openEditUserDialog(event: TableEditRowEvent<GetAllUsersItem>) {
    this.dialog
      .open(UserEditDialogComponent, {
        data: {
          editingItem: event.row,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result == 'administration.success.user.update_user') {
          this.tableUsers.refresh();
        }
      });
  }

  openDeleteUserDialog(event: TableEditRowEvent<UserType>) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      minWidth: 400,
    });

    this.subs.sink = dialogRef.afterClosed().subscribe((result) => {
      if (result === true) {
        this.subs.sink = this.userHttpService.deleteUser(event.row.id).subscribe((result) => {
          this.onDeleteResponse(result, this.tableUsers);
        });
      }
    });
  }

  onDeleteResponse(res: Response<any>, tableRef) {
    if (res.isSuccess) {
      this.notificationService.showSuccess(this.translocoService.translate('shared.common.item_delete_successfully'));
      tableRef.refresh();
    } else if (res.errors && res.errors.validation && res.errors.validation.length > 0) {
      this.notificationService.showError(this.translocoService.translate(res.errors.validation[0]));
    } else {
      this.notificationService.showError('shared.common.error');
    }
  }
}
