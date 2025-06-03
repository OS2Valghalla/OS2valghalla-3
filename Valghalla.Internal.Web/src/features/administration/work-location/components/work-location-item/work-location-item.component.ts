import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { combineLatest } from 'rxjs';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSelectionList } from '@angular/material/list';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { AreaHttpService } from '../../../area/services/area-http.service';
import { TaskTypeHttpService } from '../../../task-type/services/task-type-http.service';
import { TeamHttpService } from '../../../teams/services/teams-http.service';
import { WorkLocationHttpService } from '../../services/work-location-http.service';
import { AreaListingItem } from '../../../area/models/area-listing-item';
import { TaskTypeListingItem } from '../../../task-type/models/task-type-listing-item';
import { Team } from '../../../teams/models/team';
import { WorkLocationDetails } from '../../models/work-location-details';
import { CreateWorkLocationRequest } from '../../models/create-work-location-request';
import { UpdateWorkLocationRequest } from '../../models/update-work-location-request';
import { ElectionDetails } from 'src/features/administration/election/models/election-details';
import { ElectionHttpService } from 'src/features/administration/election/services/election-http.service';

@Component({
  selector: 'app-admin-work-location-item',
  templateUrl: './work-location-item.component.html',
  providers: [WorkLocationHttpService, AreaHttpService, TaskTypeHttpService, TeamHttpService, ElectionHttpService],
})
export class WorkLocationItemComponent implements AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading: boolean = true;

  item?: WorkLocationDetails;

  areas: AreaListingItem[] = [];

  elections: ElectionDetails[] = [];

  taskTypes: TaskTypeListingItem[] = [];

  teams: Team[] = [];

  readonly form = this.formBuilder.group({
    title: ['', Validators.required],
    areaId: ['', Validators.required],
    // electionId: ['', Validators.required],
    address: ['', Validators.required],
    postalCode: ['', [Validators.required, Validators.pattern(/^\d{4}$/)]],
    city: ['', Validators.required],
    voteLocation: [0],
    taskTypeIds: [],
    teamIds: [],
    responsibleIds: [[] as string[]],
  });

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  @ViewChild('selectedTaskTypes') private readonly taskTypesList: MatSelectionList;

  @ViewChild('selectedTeams') private readonly teamsList: MatSelectionList;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly workLocationHttpService: WorkLocationHttpService,
    private readonly areaHttpService: AreaHttpService,
    private readonly taskTypeHttpService: TaskTypeHttpService,
    private readonly teamHttpService: TeamHttpService,
    private readonly electionHttpService: ElectionHttpService
  ) { }

  ngAfterViewInit(): void {
    this.subs.sink = combineLatest({
      areas: this.areaHttpService.getAllAreas(),
      taskTypes: this.taskTypeHttpService.getAllTaskTypes(),
      teams: this.teamHttpService.getAllTeams(),
      elections: this.electionHttpService.getAllElections(),
      formPageState: this.formPage.state$,
    }).subscribe((v) => {
      this.areas = v.areas.data;
      this.taskTypes = v.taskTypes.data;
      this.teams = v.teams.data;
      this.elections = v.elections.data;

      this.subs.sink = this.formPage.state$.subscribe(() => {
        if (!this.formPage.isUpdateForm()) {
          this.loading = false;
          this.changeDetectorRef.detectChanges();
          return;
        }

        this.subs.sink = this.workLocationHttpService.getWorkLocationDetails(this.formPage.itemId).subscribe((res) => {
          this.loading = false;
          this.item = res.data;

          if (res.data) {
            this.form.setValue({
              title: res.data.title,
              areaId: res.data.areaId,
              address: res.data.address,
              postalCode: res.data.postalCode,
              city: res.data.city,
              voteLocation: res.data.voteLocation,
              taskTypeIds: res.data.taskTypeIds,
              teamIds: res.data.teamIds,
              responsibleIds: res.data.responsibleIds,
// electionId: res.data.electionId ? res.data.electionId : '',
            });
            this.changeDetectorRef.detectChanges();
          }
          this.taskTypesList.options.forEach((option) => {
            if (this.form.controls.taskTypeIds.value.indexOf(option.value) > -1) {
              option.selected = true;
            }
          });
          this.teamsList.options.forEach((option) => {
            if (this.form.controls.teamIds.value.indexOf(option.value) > -1) {
              option.selected = true;
            }
          });
        });
      });
    });

  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  changeSelectedTaskTypes() {
    const selectedOptions = [];
    this.taskTypesList.selectedOptions.selected.forEach((selectedTaskType) => {
      selectedOptions.push(selectedTaskType.value);
    });
    this.form.controls.taskTypeIds.patchValue(selectedOptions);
    this.form.markAsDirty();
  }

  changeSelectedTeams() {
    const selectedOptions = [];
    this.teamsList.selectedOptions.selected.forEach((selectedTeam) => {
      selectedOptions.push(selectedTeam.value);
    });
    this.form.controls.teamIds.patchValue(selectedOptions);
    this.form.markAsDirty();
  }

  createWorkLocation(event: FormPageEvent) {
    const request: CreateWorkLocationRequest = {
      title: this.form.value.title,
      areaId: this.form.value.areaId,
      address: this.form.value.address,
      postalCode: this.form.value.postalCode,
      city: this.form.value.city,
      voteLocation: this.form.value.voteLocation,
      taskTypeIds: this.form.value.taskTypeIds ? this.form.value.taskTypeIds : [],
      teamIds: this.form.value.teamIds ? this.form.value.teamIds : [],
      responsibleIds: this.form.value.responsibleIds ? this.form.value.responsibleIds : []
    };

    this.subs.sink = event.pipe(this.workLocationHttpService.createWorkLocation(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  updateWorkLocation(event: FormPageEvent) {
    const request: UpdateWorkLocationRequest = {
      id: this.formPage.itemId,
      title: this.form.value.title,
      areaId: this.form.value.areaId,
      address: this.form.value.address,
      postalCode: this.form.value.postalCode,
      city: this.form.value.city,
      voteLocation: this.form.value.voteLocation,
      taskTypeIds: this.form.value.taskTypeIds ? this.form.value.taskTypeIds : [],
      teamIds: this.form.value.teamIds ? this.form.value.teamIds : [],
      responsibleIds: this.form.value.responsibleIds ? this.form.value.responsibleIds : []
    };

    this.subs.sink = event.pipe(this.workLocationHttpService.updateWorkLocation(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  deleteWorkLocation(event: FormPageEvent) {
    this.subs.sink = event.pipe(this.workLocationHttpService.deleteWorkLocation(this.formPage.itemId)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
      else {
        this.formPage.deleting = false;
      }
    });
  }

  cancel() {
    this.formPage.navigate();
  }
}
