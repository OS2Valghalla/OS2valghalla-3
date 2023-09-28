import { Observable } from 'rxjs';
import { Response } from '../respone';

export interface WizardEvent {
  pipe(observable: Observable<Response<unknown>>): Observable<Response<unknown>>;
}
