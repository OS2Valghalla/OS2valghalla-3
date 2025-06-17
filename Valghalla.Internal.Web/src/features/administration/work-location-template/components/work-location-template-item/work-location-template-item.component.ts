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
import { WorkLocationTemplateHttpService } from '../../services/work-location-template-http.service';
import { AreaListingItem } from '../../../area/models/area-listing-item';
import { TaskTypeListingItem } from '../../../task-type/models/task-type-listing-item';
import { Team } from '../../../teams/models/team';
import { WorkLocationTemplateDetails } from '../../models/work-location-template-details';
import { CreateWorkLocationTemplateRequest } from '../../models/create-work-location-template-request';
import { UpdateWorkLocationTemplateRequest } from '../../models/update-work-location-template-request';
import { ElectionDetails } from 'src/features/administration/election/models/election-details';
import { ElectionHttpService } from 'src/features/administration/election/services/election-http.service';

@Component({
  selector: 'app-admin-work-location-template-item',
  templateUrl: './work-location-template-item.component.html',
  providers: [WorkLocationTemplateHttpService, AreaHttpService, TaskTypeHttpService, TeamHttpService, ElectionHttpService],
})
export class WorkLocationTemplateItemComponent implements AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading: boolean = true;

  item?: WorkLocationTemplateDetails;

  areas: AreaListingItem[] = [];

  elections: ElectionDetails[] = [];

  taskTypes: TaskTypeListingItem[] = [];

  teams: Team[] = [];

  readonly form = this.formBuilder.group({
    title: ['', Validators.required],
    areaId: ['', Validators.required],
    address: ['', Validators.required],
    postalCode: ['', [Validators.required, Validators.pattern(/^\d{4}$/)]],
    city: ['', Validators.required],
    voteLocation: [0],
  });

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly workLocationTemplateHttpService: WorkLocationTemplateHttpService,
    private readonly areaHttpService: AreaHttpService,
    private readonly taskTypeHttpService: TaskTypeHttpService,
    private readonly teamHttpService: TeamHttpService,
    private readonly electionHttpService: ElectionHttpService
  ) { }

  ngAfterViewInit(): void {
    this.subs.sink = combineLatest({
      areas: this.areaHttpService.getAllAreas(),
      formPageState: this.formPage.state$,
    }).subscribe((v) => {
      this.areas = v.areas.data;

      this.subs.sink = this.formPage.state$.subscribe(() => {
        if (!this.formPage.isUpdateForm()) {
          this.loading = false;
          this.changeDetectorRef.detectChanges();
          return;
        }

        this.subs.sink = this.workLocationTemplateHttpService.getWorkLocationTemplateDetails(this.formPage.itemId).subscribe((res) => {
          this.loading = false;
          this.item = res.data;

          if (res.data) {
            this.form.setValue({
              title: res.data.title,
              areaId: res.data.areaId,
              address: res.data.address,
              postalCode: res.data.postalCode,
              city: res.data.city,
              voteLocation: res.data.voteLocation
            });
            this.changeDetectorRef.detectChanges();
          }
        });
      });
    });

  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }


  createWorkLocationTemplate(event: FormPageEvent) {
    const request: CreateWorkLocationTemplateRequest = {
      title: this.form.value.title,
      areaId: this.form.value.areaId,
      address: this.form.value.address,
      postalCode: this.form.value.postalCode,
      city: this.form.value.city,
      voteLocation: this.form.value.voteLocation
    };

    this.subs.sink = event.pipe(this.workLocationTemplateHttpService.createWorkLocationTemplate(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  updateWorkLocationTemplate(event: FormPageEvent) {
    const request: UpdateWorkLocationTemplateRequest = {
      id: this.formPage.itemId,
      title: this.form.value.title,
      areaId: this.form.value.areaId,
      address: this.form.value.address,
      postalCode: this.form.value.postalCode,
      city: this.form.value.city,
      voteLocation: this.form.value.voteLocation
    };

    this.subs.sink = event.pipe(this.workLocationTemplateHttpService.updateWorkLocationTemplate(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  deleteWorkLocationTemplate(event: FormPageEvent) {
    this.subs.sink = event.pipe(this.workLocationTemplateHttpService.deleteWorkLocationTemplate(this.formPage.itemId)).subscribe((res) => {
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
