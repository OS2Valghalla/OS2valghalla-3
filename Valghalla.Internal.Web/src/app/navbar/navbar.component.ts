import { Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { Role } from '../../shared/constants/role';
import { GlobalStateService } from '../global-state.service';
import { ElectionShared } from 'src/shared/models/election/election-shared';

@Component({
  selector: 'app-navbar',
  templateUrl: 'navbar.component.html',
  styleUrls: ['navbar.component.scss'],
})
export class NavbarComponent implements OnDestroy, OnInit {
  private subs = new SubSink();
  @ViewChild('menuButton') menuButton: ElementRef;
  @Input() isDesktopView: boolean;
  activeElection: ElectionShared;
  enteredButton = false;
  isMatMenuOpen = false;
  isSidenavOpen = false;
  prevButtonTrigger;
  public role = Role.administrator;

  constructor(private globalStateService: GlobalStateService) {}

  ngOnInit() {
    this.getGlobalData();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  getGlobalData() {
    this.subs.sink = this.globalStateService.election$.subscribe((response) => {
      this.activeElection = response;
    });
  }

  menuenter() {
    this.isMatMenuOpen = true;
  }

  toggleSitenavOpen() {
    this.isSidenavOpen = !this.isSidenavOpen;
  }

  menuLeave(trigger) {
    setTimeout(() => {
      if (!this.enteredButton) {
        this.isMatMenuOpen = false;
        trigger.closeMenu();
      } else {
        this.isMatMenuOpen = false;
      }
    }, 80);
  }

  buttonEnter(trigger) {
    setTimeout(() => {
      if (this.prevButtonTrigger && this.prevButtonTrigger != trigger) {
        this.prevButtonTrigger.closeMenu();
        this.prevButtonTrigger = trigger;
        this.isMatMenuOpen = false;
        trigger.openMenu();
      } else if (!this.isMatMenuOpen) {
        this.enteredButton = true;
        this.prevButtonTrigger = trigger;
        trigger.openMenu();
      } else {
        this.enteredButton = true;
        this.prevButtonTrigger = trigger;
      }
    }, 400);
  }

  buttonLeave(trigger) {
    setTimeout(() => {
      if (this.enteredButton && !this.isMatMenuOpen) {
        trigger.closeMenu();
      }
      if (!this.isMatMenuOpen) {
        trigger.closeMenu();
      } else {
        this.enteredButton = false;
      }
    }, 400);
  }
}
