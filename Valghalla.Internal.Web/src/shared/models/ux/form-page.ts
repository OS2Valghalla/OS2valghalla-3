import { Observable } from 'rxjs';
import { Response } from '../respone';

export interface FormPageEvent {
  pipe(observable: Observable<Response<unknown>>): Observable<Response<unknown>>;
}
