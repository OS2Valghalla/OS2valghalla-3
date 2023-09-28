import { InjectionToken, ViewContainerRef } from '@angular/core';
import { ReplaySubject } from 'rxjs';

export const TOAST_CONTAINER = new InjectionToken<ReplaySubject<ViewContainerRef>>('ToastContainer');
