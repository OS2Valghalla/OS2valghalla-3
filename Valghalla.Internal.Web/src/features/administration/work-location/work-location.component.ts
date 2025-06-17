import { Component, OnDestroy, ViewChild } from '@angular/core';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { GlobalStateService } from '../../../app/global-state.service';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { WorkLocationListingItem } from './models/work-location-listing-item';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { WorkLocationHttpService } from './services/work-location-http.service';
import { TableComponent } from 'src/shared/components/table/table.component';
import { ElectionDetails } from '../election/models/election-details';
import { ElectionHttpService } from '../election/services/election-http.service';
import { WorkLocationTemplateHttpService } from '../work-location-template/services/work-location-template-http.service';
import { WorkLocationTemplateDetails } from '../work-location-template/models/work-location-template-details';

@Component({
  selector: 'app-admin-work-location',
  templateUrl: './work-location.component.html',
  providers: [WorkLocationHttpService, ElectionHttpService, WorkLocationTemplateHttpService],
})
export class WorkLocationComponent implements OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  data: Array<WorkLocationListingItem> = [];
  allData: Array<WorkLocationListingItem> = [];
  elections: ElectionDetails[] = [];
  workLocationTemplates: Array<WorkLocationTemplateDetails> = [];
  selectedElectionId: string | null = null;
  selectedTemplateId: string | null = null;

  @ViewChild(TableComponent) private readonly table: TableComponent<WorkLocationListingItem>;

  constructor(
    private readonly router: Router,
    private readonly workLocationHttpService: WorkLocationHttpService,
    private readonly workLocationTemplateHttpService: WorkLocationTemplateHttpService,
    private readonly electionHttpService: ElectionHttpService
  ) { }
  ngAfterViewInit(): void {
    this.electionHttpService.getAllElections().subscribe((response) => {
      this.elections = response.data;
      this.loading = false;
    });
    this.workLocationTemplateHttpService.getAllWorkLocationTemplates().subscribe((response) => {
      this.workLocationTemplates = response.data;
      this.loading = false;
    });

  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onModelChange(items: WorkLocationListingItem[]) {
    this.allData = items;
    this.applyElectionFilter();
  }

  onQuery(event: QueryEvent<QueryForm>) {
    if (!event.query.order) {
      event.query.order = {
        name: 'title',
        descending: false,
      };
    }
    if (this.selectedElectionId) {
      event.query.electionId = this.selectedElectionId;
    }
    if (this.selectedTemplateId) {
      event.query.templateId = this.selectedTemplateId;
    }
    event.execute('administration/worklocation/queryworklocationlisting', event.query);
  }

  applyElectionFilter() {
    if (this.selectedElectionId) {
      this.data = this.allData.filter(item => item.electionId === this.selectedElectionId);
    } else {
      this.data = [...this.allData];
    }
    this.applyTemplateFilter();
  }

  applyTemplateFilter() {
    if (this.selectedTemplateId) {
      this.data = this.data.filter(item => item.templateId === this.selectedTemplateId);
    } else {
    }
  }

  onElectionChange(electionId: string) {
    this.selectedElectionId = electionId;
    this.applyElectionFilter();
  }

  onTemplateChange(templateId: string) {
    this.selectedTemplateId = templateId;
    this.applyElectionFilter();
  }
  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/worklocation/getworklocationlistingqueryform');
  }

  openAddWorkLocation() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.WorkLocation, RoutingNodes.Link_Create]);
  }

  openEditWorkLocation(event: TableEditRowEvent<WorkLocationListingItem>) {
    this.router.navigate([
      RoutingNodes.Administration,
      RoutingNodes.WorkLocation,
      RoutingNodes.Link_Edit,
      event.row.id,
    ]);
  }

  deleteWorkLocation(event: TableEditRowEvent<WorkLocationListingItem>) {
    this.subs.sink = this.workLocationHttpService.deleteWorkLocation(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  }
}
