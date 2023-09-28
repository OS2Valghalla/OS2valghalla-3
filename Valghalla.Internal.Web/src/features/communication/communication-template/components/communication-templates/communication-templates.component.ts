import { Component, OnDestroy, ViewChild } from '@angular/core';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { TranslocoService } from '@ngneat/transloco';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { FreeTextSearchValue } from 'src/shared/query-engine/models/free-text-search-value';
import { SingleSelectionFilterValue } from 'src/shared/query-engine/models/single-selection-filter-value';
import { TableComponent } from 'src/shared/components/table/table.component';
import { CommunicationTemplateTypes } from 'src/shared/constants/communication-template-types';
import { CommunicationHttpService } from '../../services/communication-http.service';
import { CommunicationTemplateListingItem } from '../../models/communication-template-listing-item';

interface CommunicationTemplateQueryForm extends QueryForm {
  title?: FreeTextSearchValue;
  templateType?: SingleSelectionFilterValue<string>;
}

@Component({
  selector: 'app-communication-templates',
  templateUrl: './communication-templates.component.html',
  providers: [CommunicationHttpService],
})
export class CommunicationTemplatesComponent implements OnDestroy {
  private readonly subs = new SubSink();

  loading = true;

  data: Array<CommunicationTemplateListingItem> = [];

  queryTyping: CommunicationTemplateQueryForm;

  readonly templateTypes = [
    { id: CommunicationTemplateTypes.DigitalPost, title: 'communication.template_type.digital_post' },
    { id: CommunicationTemplateTypes.Email, title: 'communication.template_type.email' },
    { id: CommunicationTemplateTypes.SMS, title: 'communication.template_type.sms' },
  ];

  @ViewChild(TableComponent) private readonly table: TableComponent<CommunicationTemplateListingItem>;

  constructor(
    private readonly translocoService: TranslocoService,
    private readonly router: Router,
    private readonly communicationHttpService: CommunicationHttpService
  ) {}

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
    event.execute('administration/communication/querycommunicationtemplatelisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<any>) {
    event.execute('administration/communication/getcommunicationtemplatelistingqueryform', {});
  }

  getTemplateTypeName(row: CommunicationTemplateListingItem) {
    var found = this.templateTypes.filter((f) => f.id == row.templateType);
    var templateTypeName = found && found.length > 0 ? this.translocoService.translate(found[0].title) : '';
    row.templateTypeName = templateTypeName;
    return templateTypeName;
  }

  openAddCommunicationTemplate() {
    this.router.navigate([
      RoutingNodes.Communication,
      RoutingNodes.CommunicationTemplates,
      RoutingNodes.Link_Create
    ]);
  }

  openEditCommunicationTemplate(event: TableEditRowEvent<CommunicationTemplateListingItem>) {
    this.router.navigate([
      RoutingNodes.Communication,
      RoutingNodes.CommunicationTemplates,
      RoutingNodes.Link_Edit,
      event.row.id,
    ]);
  }

  deleteCommunicationTemplate(event: TableEditRowEvent<CommunicationTemplateListingItem>) {
    this.subs.sink = this.communicationHttpService.deleteCommunicationTemplate(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  }
}
