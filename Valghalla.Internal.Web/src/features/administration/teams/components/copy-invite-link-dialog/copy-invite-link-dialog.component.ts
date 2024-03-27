import { Component, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppHttpService } from 'src/app/app-http.service';
import { GlobalStateService } from 'src/app/global-state.service';
import { CopyInviteLinkDialogData } from 'src/shared/models/ux/copy-invite-link-dialog';
import { Clipboard } from '@angular/cdk/clipboard';

@Component({
  selector: 'app-copy-invite-link-dialog',
  templateUrl: './copy-invite-link-dialog.component.html',
})
export class CopyInviteLinkDialogComponent {
  teamName: string = this.data.name;

  teamLink: string = this.data.link;

  constructor(private clipboard: Clipboard, private dialogRef: MatDialogRef<CopyInviteLinkDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: CopyInviteLinkDialogData, private formBuilder: FormBuilder, private globalStateService: GlobalStateService, private appHttpService: AppHttpService) {}

  onClose() {
    this.dialogRef.close(false);
  }

  onCopy() {
    this.clipboard.copy(this.teamLink);
  }
}
