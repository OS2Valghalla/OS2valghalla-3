import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import {
  MatSnackBarRef,
  MAT_SNACK_BAR_DATA,
} from '@angular/material/snack-bar';
import { SnackbarData, SnackTypes } from '../../models/snackbar-data';

@Component({
  selector: 'app-snackbar',
  templateUrl: './snackbar.component.html',
  styleUrls: ['./snackbar.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class AppSnackbarComponent implements OnInit {
  icon: string;

  constructor(
    public snackBarRef: MatSnackBarRef<AppSnackbarComponent>,
    @Inject(MAT_SNACK_BAR_DATA) public data: SnackbarData,
  ) {}

  ngOnInit(): void {
    switch (this.data.snackType) {
      case SnackTypes.Success:
        this.icon = 'check_circle_outline';
        break;
      case SnackTypes.Error:
        this.icon = 'highlight_off';
        break;
      case SnackTypes.Warning:
        this.icon = 'warning_outline';
        break;
      default:
        this.icon = 'info';
        break;
    }
  }
}
