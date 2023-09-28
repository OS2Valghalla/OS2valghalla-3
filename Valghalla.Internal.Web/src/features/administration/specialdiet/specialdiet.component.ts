import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SpecialDietHttpService } from './services/specialdiet-http.service';
import { TableComponent } from 'src/shared/components/table/table.component';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { SpecialDiet } from './models/specialdiet';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-admin-specialdiet',
  templateUrl: './specialdiet.component.html',
  providers: [SpecialDietHttpService],
})
export class SpecialDietComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  data: Array<SpecialDiet> = [];

  @ViewChild('tableSpecialDiets') private readonly tableSpecialDiets: TableComponent<SpecialDiet>;

  constructor(private router: Router, private specialDietHttpService: SpecialDietHttpService) {}

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
    event.execute('administration/specialdiet/queryspecialdietlisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/specialdiet/getspecialdietlistingqueryform');
  }

  deleteSpecialDiet(event: TableEditRowEvent<SpecialDiet>) {
    this.subs.sink = this.specialDietHttpService.deleteSpecialDiet(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.tableSpecialDiets.refresh();
      }
    });
  }

  openAddSpecialDiet() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.SpecialDiet, RoutingNodes.Link_Create]);
  }

  openEditSpecialDiet(event: TableEditRowEvent<SpecialDiet>) {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.SpecialDiet, RoutingNodes.Link_Edit, event.row.id]);
  }
}
