import { User } from './user';
import { AppElection } from './app-election';

export interface AppContext {
  user: User;
  election: AppElection;
  municipalityName: string;
  externalWebUrl: string;
}
