import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Role } from 'src/shared/constants/role';

@Component({
  selector: 'app-autosave-number-input',
  templateUrl: './autosave-number-input.component.html',
  styleUrls: ['./autosave-number-input.component.scss'],
})
export class AutosaveNumberInputComponent implements OnInit {
  @Input() id: string;
  @Input() value: number;
  @Output() valueChange: EventEmitter<number> = new EventEmitter<number>();
  @Input() callbackFunction?: (itemId: string, outputValue: number) => void;
  @Input() minValue?: number;
  @Input() maxValue?: number;
  @Input() callBackWaitingTime?: number = 500;
  @Input() textboxOnly?: boolean;
  timeoutRef: ReturnType<typeof setTimeout>;
  public role = Role.editor;

  ngOnInit() {
    if (this.maxValue && this.minValue && this.maxValue < this.minValue) {
      this.maxValue = this.minValue;
    }
  }

  handleMinus() {
    this.value--;
    if (this.minValue != null && this.value < this.minValue) {
      this.value = this.minValue;
    }
    clearTimeout(this.timeoutRef);
    this.timeoutRef = setTimeout(() => {
      this.valueChange.emit(this.value);
      if (this.callbackFunction) this.callbackFunction(this.id, this.value);
    }, this.callBackWaitingTime);
  }

  handlePlus() {
    this.value++;
    if (this.maxValue != null && this.value > this.maxValue) {
      this.value = this.maxValue;
    }
    clearTimeout(this.timeoutRef);
    this.timeoutRef = setTimeout(() => {
      this.valueChange.emit(this.value);
      if (this.callbackFunction) this.callbackFunction(this.id, this.value);
    }, this.callBackWaitingTime);
  }

  onValueChanged() {
    if (this.minValue != null && this.value < this.minValue) {
      this.value = this.minValue;
    }
    if (this.maxValue != null && this.value > this.maxValue) {
      this.value = this.maxValue;
    }
    clearTimeout(this.timeoutRef);
    this.timeoutRef = setTimeout(() => {
      this.valueChange.emit(this.value);
      if (this.callbackFunction) this.callbackFunction(this.id, this.value);
    }, this.callBackWaitingTime);
  }
}
