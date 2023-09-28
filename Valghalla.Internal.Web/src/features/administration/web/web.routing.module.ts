import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { WebLandingComponent } from '../web/landing.component';
import { WebFrontPageComponent } from '../web/components/front-page.component';
import { WebFAQPageComponent } from '../web/components/faq-page.component';
import { WebContactInformationComponent } from '../web/components/contact-information.component';
import { WebDeclarationOfConsentComponent } from '../web/components/declaration-of-consent-page.component';
import { WebDisclosureStatementComponent } from '../web/components/disclosure-statement-page.component';

const routes: Routes = [
    {
        path: '',
        title: 'administration.web.page_title',
        data: { breadcrumb: 'administration.web.page_title' },
        component: WebLandingComponent,
      },
    {
        path: RoutingNodes.Web_Front,
        title: 'app.navigation.web.front.title',
        data: { breadcrumb: 'app.navigation.web.front.title' },
        component: WebFrontPageComponent,
    },
    {
        path: RoutingNodes.Web_FAQ,
        title: 'app.navigation.web.faq.title',
        data: { breadcrumb: 'app.navigation.web.faq.title' },
        component: WebFAQPageComponent,
    },
    {
      path: RoutingNodes.Web_DisclosureStatement,
      title: 'app.navigation.web.disclosure_statement.title',
      data: { breadcrumb: 'app.navigation.web.disclosure_statement.title' },
      component: WebDisclosureStatementComponent,
    },
    {
      path: RoutingNodes.Web_DeclarationOfConsent,
      title: 'app.navigation.web.declaration_of_consent.title',
      data: { breadcrumb: 'app.navigation.web.declaration_of_consent.title' },
      component: WebDeclarationOfConsentComponent,
    },
    {
      path: RoutingNodes.Web_ContactInformation,
      title: 'app.navigation.web.contact_information.title',
      data: { breadcrumb: 'app.navigation.web.contact_information.title' },
      component: WebContactInformationComponent,
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WebRoutingModule {}