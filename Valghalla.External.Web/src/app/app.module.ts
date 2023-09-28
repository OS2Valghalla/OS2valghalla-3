import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ErrorHandler, Injectable, NgModule } from '@angular/core';
import { DATE_PIPE_DEFAULT_OPTIONS } from '@angular/common';
import { BrowserModule, Title } from '@angular/platform-browser';
import { RouterStateSnapshot, TitleStrategy } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { SharedModule } from 'src/shared/shared.module';
import { AuthModule } from './auth/auth.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { GlobalErrorHandler } from './global-error-handler.interceptor';
import { TranslocoRootModule } from './transloco-root.module';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { AppIconCollection } from './icon-collection/app-icon-collection.component';
import { ReactiveFormsModule } from '@angular/forms';
import { DkfdsModule } from 'src/shared/dkfds/dkfds.module';
import { ContactUsComponent } from '../features/contact-us/contact-us.component';
import { PrivacyPolciyComponent } from '../features/privacy-policy/privacy-policy.component';
import { FilteredTasksComponent } from '../features/tasks/components/filtered-tasks/filtered-tasks.component';
import { TeamInvitationComponent } from '../features/unprotected/team-invitation/team-invitation.component';
import { TOAST_CONTAINER } from 'src/shared/constants/injection-token';
import { ReplaySubject } from 'rxjs';
import { dateFormat } from 'src/shared/constants/date';
import { TasksModule } from 'src/features/tasks/tasks.module';
import { AppHttpInterceptor } from './app-http.interceptor';

@Injectable()
export class AppPageTitleStrategy extends TitleStrategy {
  constructor(private translocoService: TranslocoService, private readonly title: Title) {
    super();
  }

  override updateTitle(routerState: RouterStateSnapshot) {
    const title = this.buildTitle(routerState);
    const localizedTitle = this.translocoService.translate(title);
    const productTitle = this.translocoService.translate('shared.page_title');
    this.title.setTitle(localizedTitle + ' - ' + productTitle);
  }
}

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    AppIconCollection,
    ContactUsComponent,
    PrivacyPolciyComponent,
    FilteredTasksComponent,
    TeamInvitationComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AuthModule,
    SharedModule,
    ReactiveFormsModule,
    DkfdsModule,
    TranslocoRootModule,
    BrowserAnimationsModule,
    TasksModule,
  ],
  providers: [
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler,
    },
    {
      provide: TitleStrategy,
      useClass: AppPageTitleStrategy,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AppHttpInterceptor,
      multi: true,
    },
    {
      provide: TOAST_CONTAINER,
      useValue: new ReplaySubject(1),
    },
    {
      provide: DATE_PIPE_DEFAULT_OPTIONS,
      useValue: { dateFormat: dateFormat },
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
