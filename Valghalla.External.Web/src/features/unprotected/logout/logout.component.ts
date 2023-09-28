import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-unprotected-logout',
  templateUrl: './logout.component.html',
})
export class LogoutComponent implements OnInit {
  profileRemovalAlertVisible: boolean = false;

  constructor(private readonly route: ActivatedRoute) {}

  ngOnInit(): void {
    this.profileRemovalAlertVisible = this.route.snapshot.queryParamMap.get('profile-deleted') == 'true';
  }
}
