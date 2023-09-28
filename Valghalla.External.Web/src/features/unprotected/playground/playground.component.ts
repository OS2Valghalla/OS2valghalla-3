import { AfterViewInit, Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NotificationService } from 'src/shared/services/notification.service';

@Component({
  selector: 'app-playground-landing',
  templateUrl: './playground.component.html',
})
export class PlaygroundComponent implements AfterViewInit {
  readonly form = this.formBuilder.group({
    text: ['', Validators.required],
    textLimit: ['', Validators.required],
    number: [undefined, Validators.required],
    date: [undefined, Validators.required],
    datePicker: [undefined as any, Validators.required],
    selection: [undefined, Validators.required],
    inputWithSuffix: [undefined, Validators.required],
    radioValue: [undefined, Validators.required],
    textarea: ['', Validators.required],
    checkboxValue: [undefined, Validators.required],
    file: [undefined, Validators.required]
  });

  constructor(private readonly formBuilder: FormBuilder, private readonly notificationService: NotificationService) {}

  ngAfterViewInit(): void {
    this.form.controls.datePicker.setValue("2023-07-26T17:00:00Z");
  }
}
