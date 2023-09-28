import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ElectionTypeHttpService } from './services/election-type-http.service';
import { TableComponent } from 'src/shared/components/table/table.component';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { ElectionType } from './models/election-type';
import { QueryEvent, QueryForm } from 'src/shared/query-engine/models/query-form';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-admin-election-type',
  templateUrl: './election-type.component.html',
  providers: [ElectionTypeHttpService],
})
export class ElectionTypeComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  data: Array<ElectionType> = [];

  @ViewChild('tableElectionTypes') private readonly tableElectionTypes: TableComponent<ElectionType>;

  constructor(private router: Router, private electionTypeHttpService: ElectionTypeHttpService) {}

  ngOnInit(): void {}

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<QueryForm>) {
    if (!event.query.order) {
      event.query.order = {
        name: 'title',
        descending: false,
      };
    }
    event.execute('administration/electiontype/queryelectiontypelisting', event.query);
  }

  deleteElectionType(event: TableEditRowEvent<ElectionType>) {
    this.subs.sink = this.electionTypeHttpService.deleteElectionType(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.tableElectionTypes.refresh();
      }
    });
  }

  openAddElectionType() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.ElectionType, RoutingNodes.Link_Create]);
  }

  openEditElectionType(event: TableEditRowEvent<ElectionType>) {
    this.router.navigate([
      RoutingNodes.Administration,
      RoutingNodes.ElectionType,
      RoutingNodes.Link_Edit,
      event.row.id,
    ]);
  }
}
