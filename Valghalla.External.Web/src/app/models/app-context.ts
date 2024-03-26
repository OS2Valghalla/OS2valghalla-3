import { ElectionCommitteeContactInformationPage } from 'src/shared/models/web-page';

export interface AppContext {
  electionActivated: boolean;
  faqPageActivated: boolean;
  webPage?: ElectionCommitteeContactInformationPage;
}
