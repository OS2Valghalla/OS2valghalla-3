import { MediaMatcher } from '@angular/cdk/layout';
import {
  AfterViewInit,
  AfterViewChecked,
  ChangeDetectorRef,
  Component,
  Inject,
  OnDestroy,
  OnInit,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { NavigationStart, Router } from '@angular/router';
import { ReplaySubject } from 'rxjs';
import { SIDE_MODAL } from 'src/shared/constants/injection-tokens';
import { SideModal } from 'src/shared/models/ux/side-modal';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy, AfterViewInit, AfterViewChecked {
  private readonly subs = new SubSink();

  mobileQuery: MediaQueryList;
  isUnprotectedRoute = false;

  private mobileQueryListener: () => void;

  @ViewChild('sidenav') sidenav: MatSidenav;
  @ViewChild('sideModal', { read: ViewContainerRef }) sideModalRef: ViewContainerRef;

  constructor(
    media: MediaMatcher,
    private changeDetectorRef: ChangeDetectorRef,
    private readonly router: Router,
    @Inject(SIDE_MODAL) private sideModal: ReplaySubject<SideModal>,
  ) {
    this.mobileQuery = media.matchMedia('(max-width: 1025px)');
    this.mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addEventListener('change', (this.mobileQuery, this.mobileQueryListener));

    this.router.events.forEach((event) => {
      if (event instanceof NavigationStart) {
        this.isUnprotectedRoute = event.url.startsWith('/_');
      }
    });
  }

  ngAfterViewInit(): void {
    this.sideModal.next({
      viewContainerRef: this.sideModalRef,
      component: this.sidenav,
    });
  }

  ngAfterViewChecked(){
    this.changeDetectorRef.detectChanges();
 }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  ngOnInit() {}
}
