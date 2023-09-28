import { MediaMatcher } from '@angular/cdk/layout';
import {
  ChangeDetectorRef,
  Component,
  OnInit,
  OnDestroy,
  AfterViewChecked,
  ViewChild,
  ViewContainerRef,
  Inject,
  AfterViewInit,
} from '@angular/core';
import { ReplaySubject, take } from 'rxjs';
import { TOAST_CONTAINER } from 'src/shared/constants/injection-token';
import { SubSink } from 'subsink';
import { GlobalStateService } from './global-state.service';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { Router, RoutesRecognized } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy, AfterViewChecked, AfterViewInit {
  private readonly subs = new SubSink();

  mobileQuery: MediaQueryList;
  nonActiveElectionMsgVisible: boolean = false;

  private mobileQueryListener: () => void;

  @ViewChild('toastContainer', { read: ViewContainerRef }) toastContainerRef: ViewContainerRef;

  constructor(
    media: MediaMatcher,
    private readonly changeDetectorRef: ChangeDetectorRef,
    @Inject(TOAST_CONTAINER) private toastContainerInjection: ReplaySubject<ViewContainerRef>,
    private readonly globalStateService: GlobalStateService,
    private readonly router: Router,
  ) {
    this.mobileQuery = media.matchMedia('(max-width: 1025px)');
    this.mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addEventListener('change', (this.mobileQuery, this.mobileQueryListener));
  }

  ngOnInit() {
    this.subs.sink = this.router.events.subscribe((event) => {
      this.globalStateService.electionActivated$.pipe(take(1)).subscribe((electionActivated) => {
        if (event instanceof RoutesRecognized) {
          this.nonActiveElectionMsgVisible =
            !electionActivated &&
            !event.url.endsWith(RoutingNodes.ContactUs) &&
            !event.url.endsWith(RoutingNodes.Faq) &&
            !event.url.endsWith(RoutingNodes.PrivacyPolicy);
        }
      });
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.toastContainerInjection.next(this.toastContainerRef);
  }

  ngAfterViewChecked() {
    this.changeDetectorRef.detectChanges();
  }
}
