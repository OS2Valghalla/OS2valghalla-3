import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Team } from '../../models/team';
import { CreateTeamRequest } from '../../models/create-team-request';
import { UpdateTeamRequest } from '../../models/update-team-request';
import { TeamHttpService } from '../../services/teams-http.service';
import { SubSink } from 'subsink';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { FormPageEvent } from 'src/shared/models/ux/form-page';

@Component({
  selector: 'app-admin-team-item',
  templateUrl: './team-item.component.html',
  providers: [TeamHttpService],
})
export class TeamItemComponent implements AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  item: Team;

  readonly form = this.formBuilder.group({
    name: ['', Validators.required],
    shortName: ['', Validators.required],
    description: [''],
    responsibleIds: [[] as string[]],
  });

  participantPickerVisble: boolean = false;

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly teamHttpService: TeamHttpService,
  ) {}

  ngAfterViewInit(): void {
    this.subs.sink = this.formPage.state$.subscribe(() => {
      if (!this.formPage.isUpdateForm()) {
        this.loading = false;
        this.changeDetectorRef.detectChanges();
        return;
      }

      this.subs.sink = this.teamHttpService.getTeam(this.formPage.itemId).subscribe((res) => {
        this.loading = false;
        this.item = res.data;

        if (res.data) {
          this.form.setValue({
            name: res.data.name,
            shortName: res.data.shortName,
            description: res.data.description,
            responsibleIds: res.data.responsibleIds,
          });
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  createTeam(event: FormPageEvent) {
    const createRequest: CreateTeamRequest = {
      name: this.form.value.name,
      shortName: this.form.value.shortName,
      description: this.form.value.description,
      responsibleIds: this.form.value.responsibleIds,
    };

    this.subs.sink = event.pipe(this.teamHttpService.createTeam(createRequest)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  updateTeam(event: FormPageEvent) {
    const updateRequest: UpdateTeamRequest = {
      id: this.formPage.itemId,
      name: this.form.value.name,
      shortName: this.form.value.shortName,
      description: this.form.value.description,
      responsibleIds: this.form.value.responsibleIds,
    };

    this.subs.sink = event.pipe(this.teamHttpService.updateTeam(updateRequest)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  deleteTeam(event: FormPageEvent) {
    this.subs.sink = event.pipe(this.teamHttpService.deleteTeam(this.formPage.itemId)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  cancel() {
    this.formPage.navigate();
  }
}
