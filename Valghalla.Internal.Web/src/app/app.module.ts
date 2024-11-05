import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { ErrorHandler, Injectable, NgModule } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterStateSnapshot, TitleStrategy } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { ReplaySubject } from 'rxjs';
import { SIDE_MODAL } from 'src/shared/constants/injection-tokens';
import { MaterialModule } from 'src/shared/material.module';
import { SharedModule } from 'src/shared/shared.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule } from './auth/auth.module';
import { ChangeElectionDialogComponent } from './footer/change-election-dialog/change-election-dialog.component';
import { FooterComponent } from './footer/footer.component';
import { GlobalErrorHandler } from './global-error-handler.interceptor';
import { NavbarComponent } from './navbar/navbar.component';
import { TranslocoRootModule } from './transloco-root.module';
import { AppHttpInterceptor } from './app-http.interceptor';
import { GdprConfirmationDialogComponent } from './gdpr-confirmation-dialog.component';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { MatPaginationIntlService } from './mat-pagination-intl.service';

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

@NgModule({ declarations: [
        AppComponent,
        NavbarComponent,
        FooterComponent,
        ChangeElectionDialogComponent,
        GdprConfirmationDialogComponent,
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        AppRoutingModule,
        AuthModule,
        MaterialModule,
        TranslocoRootModule,
        BrowserAnimationsModule,
        SharedModule], providers: [
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
            provide: SIDE_MODAL,
            useValue: new ReplaySubject(1),
        },
        { provide: MatPaginatorIntl, useClass: MatPaginationIntlService },
        provideHttpClient(withInterceptorsFromDi()),
    ] })
export class AppModule {}
