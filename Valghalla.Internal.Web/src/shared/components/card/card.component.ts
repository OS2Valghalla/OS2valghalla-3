import { Component, ContentChildren, EventEmitter, Input, Output, QueryList } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { Role } from '../../constants/role';
import { CardButtonDirective } from './card-button.directive';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./_card-theme.scss'],
})
export class CardComponent {
  @Input() cardTitle: string;
  @Input() actionTitle: string;
  @Input() actionIcon: string;
  @Input() useAnimation: boolean;
  @Input() isLink: boolean;
  @Output() action = new EventEmitter();
  @Input() color = 'primary';
  @Input() maxWidth = '';
  @Input() minWidth = '';
  @Input() allowReader = false;
  @Input() minHeight = '';
  @Input() maxHeight = '';
  @Input() actionVisible: boolean = true;

  public role = Role.editor;
  @Input() headerVisible: boolean = true;

  @ContentChildren(CardButtonDirective, { descendants: false })
  readonly cardButtons!: QueryList<CardButtonDirective>;

  onActionClick() {
    this.action.emit({
      pipe: (observable, executor) => {
        observable = observable.pipe(
          catchError((err) => {
            return throwError(() => err);
          }),
        );

        executor(observable, {
          next: () => {},
          error: (errors) => {
            console.error(errors);
          },
        });
      },
    });
  }
}
