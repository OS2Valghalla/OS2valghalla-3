import { Component, OnDestroy, ViewChild } from '@angular/core';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TableComponent } from 'src/shared/components/table/table.component';
import { TaskTypeHttpService } from './services/task-type-http.service';
import { TaskTypeListingItem } from './models/task-type-listing-item';
import { TaskTypeTemplateDetails } from '../task-type-template/models/task-type-template-details';
import { ElectionDetails } from '../election/models/election-details';
import { TaskTypeTemplateHttpService } from '../task-type-template/services/task-type-template-http.service';
import { ElectionHttpService } from '../election/services/election-http.service';
import { AreaDetails } from '../area/models/area-details';
import { AreaHttpService } from '../area/services/area-http.service';

@Component({
  selector: 'app-admin-task-type',
  templateUrl: './task-type.component.html',
  providers: [TaskTypeHttpService, TaskTypeTemplateHttpService, ElectionHttpService, AreaHttpService],
})
export class TaskTypeComponent implements OnDestroy {

  private readonly subs = new SubSink();

  loading = true;
  data: Array<TaskTypeListingItem> = [];
  allData: Array<TaskTypeListingItem> = [];
  elections: ElectionDetails[] = [];
  areas: Array<AreaDetails> = [];
  taskTypeTemplates: Array<TaskTypeTemplateDetails> = [];
  taskTypes: TaskTypeListingItem[] = [];
  selectedElectionId: string | null = null;
  selectedTemplateId: string | null = null;
  selectedAreaId: string | null = null;

  @ViewChild(TableComponent) private readonly table: TableComponent<TaskTypeListingItem>;

  constructor(
    private readonly router: Router,
    private readonly taskTypeHttpService: TaskTypeHttpService,
    private readonly taskTypeTemplateHttpService: TaskTypeTemplateHttpService,
    private readonly electionHttpService: ElectionHttpService,
    private readonly areaHttpService: AreaHttpService
  ) { }

  ngAfterViewInit(): void {
    this.electionHttpService.getAllElections().subscribe((response) => {
      this.elections = response.data;
      this.loading = false;
    });
    this.taskTypeTemplateHttpService.getAllTaskTypeTemplates().subscribe((response) => {
      this.taskTypeTemplates = response.data;
      this.loading = false;
    });
    this.areaHttpService.getAllAreas().subscribe((response) => {
      this.areas = response.data;
      this.loading = false;
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<QueryForm>) {
    event.execute('administration/tasktype/querytasktypelisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/tasktype/gettasktypelistingqueryform');
  }

  openAddTaskType() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.TaskType, RoutingNodes.Link_Create]);
  }

  openEditTaskType(event: TableEditRowEvent<TaskTypeListingItem>) {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.TaskType, RoutingNodes.Link_Edit, event.row.id]);
  }

  deleteTaskType(event: TableEditRowEvent<TaskTypeListingItem>) {
    this.subs.sink = this.taskTypeHttpService.deleteTaskType(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  }

  onTemplateChange(templateId: string) {
    this.selectedTemplateId = templateId;
    this.applyFilters();
  }
  onModelChange(items: TaskTypeListingItem[]) {
    this.allData = items;
    this.applyFilters();
  }
  onElectionChange(electionId: string) {
    this.selectedElectionId = electionId;
    this.applyFilters();
  }
  onAreaChange(areaId: string) {
    this.selectedAreaId = areaId;
    this.applyFilters();
  }
  applyFilters() {
    this.data = this.allData.filter(item => {
      const matchesElection = !this.selectedElectionId || item.electionId === this.selectedElectionId;
      const matchesTemplate = !this.selectedTemplateId || item.taskTypeTemplateId === this.selectedTemplateId;
      const matchesArea = !this.selectedAreaId || item.areaId === this.selectedAreaId;
      return matchesElection && matchesTemplate && matchesArea;
    });
  }


}
