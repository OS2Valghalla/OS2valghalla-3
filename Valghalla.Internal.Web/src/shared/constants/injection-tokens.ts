import { InjectionToken } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { SideModal } from '../models/ux/side-modal';

export const SIDE_MODAL = new InjectionToken<ReplaySubject<SideModal>>('SideModal');
