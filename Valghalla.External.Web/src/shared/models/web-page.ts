import { FileReference } from './file-reference';

export interface WebPage {
  title: string;
  pageContent: string;
  isActivated: boolean;
}

export interface ElectionCommitteeContactInformationPage {
  municipalityName: string;
  electionResponsibleApartment: string;
  address: string;
  postalCode: string;
  city: string;
  telephoneNumber?: string;
  digitalPost?: string;
  email?: string;
  logoFileReferenceId?: string;
  logoFileReference?: FileReference;
}
