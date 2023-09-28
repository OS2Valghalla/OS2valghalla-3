import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import {
  AfterViewInit,
  Component,
  EventEmitter,
  HostListener,
  Inject,
  Input,
  OnDestroy,
  Output,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { SIDE_MODAL } from 'src/shared/constants/injection-tokens';
import { SideModal } from 'src/shared/models/ux/side-modal';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-side-modal',
  templateUrl: './side-modal.component.html',
  styleUrls: ['./side-modal.component.scss']
})
export class SideModalComponent implements AfterViewInit, OnDestroy {
  private subs = new SubSink();

  private _modalVisible = false;

  @Input() label: string;
  @Input() submitLabel: string;
  @Input() loading: boolean;
  @Input() submitDisabled: boolean;
  @Input() large: boolean;
  @Input() position: 'start' | 'end' = 'end';
  @Input() disableClose: boolean = true;
  @Input() hideSubmit: boolean;
  @Input() hideHeader: boolean;
  @Input() hideAction: boolean;
  @Input()
  set value(visible: boolean) {
    this._modalVisible = visible;

    if (!this.sideModal) {
      return;
    }

    if (visible) {
      this.sideModal.component.disableClose = this.disableClose;
      this.sideModal.component.position = this.position;
    }

    this.renderContentIfNeeded();
  }

  get value(): boolean {
    return this._modalVisible;
  }

  @Output() valueChange = new EventEmitter<boolean>();
  @Output() submitEvent = new EventEmitter<void>();
  @Output() closeEvent = new EventEmitter<void>();
  @ViewChild('ref', { read: TemplateRef }) templateRef: TemplateRef<unknown>;

  private sideModal: SideModal;

  @HostListener('document:keyup.esc')
  onkeyup() {
    this.close();
  }

  constructor(
    @Inject(SIDE_MODAL)
    private readonly subject: ReplaySubject<SideModal>,
    private readonly breakpointObserver: BreakpointObserver,
  ) {}

  ngAfterViewInit(): void {
    this.subs.sink = this.subject.subscribe((sideModal) => {
      this.sideModal = sideModal;
      this.subs.sink = this.sideModal.component._closedStream.subscribe(() => {
        this.value = false;
        this.valueChange.emit(false);
      });

      this.renderContentIfNeeded();
    });

    this.subs.sink = this.breakpointObserver
      .observe([Breakpoints.XSmall, Breakpoints.Small, Breakpoints.Medium, Breakpoints.Large, Breakpoints.XLarge])
      .subscribe((state) => {
        if (
          state.breakpoints[Breakpoints.Small] ||
          (state.breakpoints[Breakpoints.XSmall] && this.sideModal?.component)
        ) {
          // TODO: This crashes the rendering after ng build
          if (this.sideModal?.component) {
            // ((this.sideModal?.component as any)._elementRef.nativeElement as HTMLElement).style.width = '100%';
          }
        } else if (this.sideModal?.component) {
          ((this.sideModal.component as any)._elementRef.nativeElement as HTMLElement).style.width = this.large
            ? '50%'
            : '35%';
        }
      });     
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  close() {
    this.value = false;
    this.valueChange.emit(false);
    this.closeEvent.emit();
  }

  submit() {
    this.submitEvent.emit();
  }

  private renderContentIfNeeded() {
    if (this._modalVisible && !this.sideModal.component.opened) {
      this.sideModal.component.opened = true;
      this.sideModal.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.sideModal.component.opened = false;
      this.sideModal.viewContainerRef.clear();
    }
  }
}
