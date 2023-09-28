import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { GlobalStateService } from '../global-state.service';
import { ElectionCommitteeContactInformationPage } from 'src/shared/models/web-page';

@Component({
  selector: 'app-footer',
  templateUrl: 'footer.component.html',
})
export class FooterComponent implements OnDestroy, OnInit {
  private readonly subs = new SubSink();

  webPage?: ElectionCommitteeContactInformationPage;

  contactUsUrl: string;

  privacyPolicyUrl: string;

  constructor(private globalStateService: GlobalStateService) {}

  ngOnInit() {
    this.contactUsUrl = RoutingNodes.ContactUs;
    this.privacyPolicyUrl = RoutingNodes.PrivacyPolicy;
    this.getGlobalData();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  getGlobalData() {
    this.subs.sink = this.globalStateService.webPage$.subscribe((webPage) => {
      this.webPage = webPage;
    });
  }
}
