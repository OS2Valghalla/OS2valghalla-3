import { UpdateWebPageRequest } from './update-web-page-request';

export interface UpdateFAQPageRequest extends UpdateWebPageRequest {            
    isActivated: boolean;
}
  