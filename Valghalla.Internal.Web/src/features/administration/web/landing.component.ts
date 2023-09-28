import { Component } from '@angular/core';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-web-landing',
  templateUrl: './landing.component.html',
})
export class WebLandingComponent {
  readonly routingNodes = [
    {
      routerLink: RoutingNodes.Web_FAQ,
      title: 'app.navigation.web.faq.title',
      icon: 'web',
      bodyMessage: 'app.navigation.web.faq.description',
    },
    {
      routerLink: RoutingNodes.Web_Front,
      title: 'app.navigation.web.front.title',
      icon: 'web',
      bodyMessage: 'app.navigation.web.front.description',
    },
    {
      routerLink: RoutingNodes.Web_ContactInformation,
      title: 'app.navigation.web.contact_information.title',
      icon: 'web',
      bodyMessage: 'app.navigation.web.contact_information.description',
    },
    {
      routerLink: RoutingNodes.Web_DisclosureStatement,
      title: 'app.navigation.web.disclosure_statement.title',
      icon: 'web',
      bodyMessage: 'app.navigation.web.disclosure_statement.description',
    },
    {
      routerLink: RoutingNodes.Web_DeclarationOfConsent,
      title: 'app.navigation.web.declaration_of_consent.title',
      icon: 'web',
      bodyMessage: 'app.navigation.web.declaration_of_consent.description',
    },
  ];
}
