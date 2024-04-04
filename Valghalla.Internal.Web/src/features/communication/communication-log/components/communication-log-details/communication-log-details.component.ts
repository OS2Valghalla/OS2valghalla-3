import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { CommunicationLogHttpService } from '../../services/communication-log-http.service';
import { CommunicationLogDetails } from '../../models/communication-log-details';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-admin-communication-log-details',
  templateUrl: './communication-log-details.component.html',
  providers: [CommunicationLogHttpService],
})
export class CommunicationLogDetailsComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  loading: boolean = true;
  item?: CommunicationLogDetails;

  constructor(private readonly route: ActivatedRoute, private readonly router: Router, private readonly communicationLogHttpService: CommunicationLogHttpService) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get(RoutingNodes.Id);
    this.subs.sink = this.communicationLogHttpService.getCommunicationLogDetails(id).subscribe((res) => {
      if (res.isSuccess) {
        this.loading = false;
        this.item = res.data;
        this.item.message = (this.item.message as any).replaceAll('\n', '<br/>');
      }
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  back() {
    this.router.navigate([RoutingNodes.Communication, RoutingNodes.CommunicationLogs]);
  }
}
