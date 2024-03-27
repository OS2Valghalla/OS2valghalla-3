import { Component, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppHttpService } from 'src/app/app-http.service';
import { GlobalStateService } from 'src/app/global-state.service';
import { CopyInviteLinkDialogData } from 'src/shared/models/ux/copy-invite-link-dialog';
import { Clipboard } from '@angular/cdk/clipboard';
import { NotificationService } from 'src/shared/services/notification.service';
import { TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'app-copy-invite-link-dialog',
  templateUrl: './copy-invite-link-dialog.component.html',
})
export class CopyInviteLinkDialogComponent {
  teamName: string = this.data.name;

  teamLink: string = this.data.link;

  constructor(private readonly translocoService: TranslocoService, private readonly notificationService: NotificationService, private clipboard: Clipboard, private dialogRef: MatDialogRef<CopyInviteLinkDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: CopyInviteLinkDialogData, private formBuilder: FormBuilder, private globalStateService: GlobalStateService, private appHttpService: AppHttpService) {}

  onClose() {
    this.dialogRef.close(false);
  }

  onCopy() {
    this.clipboard.copy(this.teamLink);
    this.dialogRef.close(false);
    this.notificationService.showSuccess(this.translocoService.translate('administration.teams.success.copy_team_link'));
  }
}
