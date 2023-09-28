import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable, catchError, map, startWith, throwError } from 'rxjs';
import { Role } from 'src/shared/constants/role';

@Component({
  selector: 'app-chips-option',
  styleUrls: ['./chips-option.component.scss'],
  templateUrl: './chips-option.component.html',
})
export class ChipsComponent implements OnInit {
  @Input() chipsLabel: string;
  @Input() selectedOptions: any[];
  @Input() options: any[];
  @Output() updateEvent = new EventEmitter();
  isEditting = false;
  formCtrl = new FormControl('');
  filteredOptions: Observable<any[]>;
  public role = Role.editor;

  ngOnInit() {
    this.filteredOptions = this.formCtrl.valueChanges.pipe(
      startWith(null),
      map((value: string | any) => {
        const name = typeof value === 'string' ? value : value?.name;
        return name ? this._filter(name as string) : this.excludeSelectedOptions().slice();
      }),
    );
  }

  private excludeSelectedOptions(): any[] {
    return this.options.filter((option) => !this.selectedOptions.map((x) => x.id).includes(option.id));
  }

  private _filter(value: string): any[] {
    const filterValue = value.toLowerCase();
    return this.excludeSelectedOptions().filter((category) => category.name.toLowerCase().includes(filterValue));
  }

  removeItem(itemId: string) {
    const index = this.selectedOptions.findIndex((x) => x.id === itemId);
    if (index >= 0) {
      this.selectedOptions.splice(index, 1);
    }
    this.formCtrl.setValue(null);
    this.updateOptions();
  }

  selectedItem(event): void {
    this.selectedOptions.push(event.option.value);
    this.formCtrl.setValue(null);
    this.updateOptions();
  }

  onEditClick() {
    this.isEditting = !this.isEditting;
  }

  updateOptions() {
    this.updateEvent.emit({
      pipe: (observable, executor) => {
        observable = observable.pipe(
          catchError((err) => {
            return throwError(() => err);
          }),
        );

        executor(observable, {
          next: (msg) => {},
          error: (errors) => {
            console.error(errors);
          },
        });
      },
    });
  }
}
