import { ElectionCommitteeContactInformationPage } from 'src/shared/models/web-page';
import { User } from './user';

export interface AppContext {
  electionActivated: boolean;
  faqPageActivated: boolean;
  user?: User;
  webPage?: ElectionCommitteeContactInformationPage;
}
