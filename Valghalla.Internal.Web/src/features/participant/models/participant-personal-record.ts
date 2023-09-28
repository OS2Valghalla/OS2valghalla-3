export interface ParticipantPersonalRecord {
  firstName?: string;
  lastName?: string;
  streetAddress?: string;
  postalCode?: string;
  city?: string;
  municipalityCode?: string;
  municipalityName?: string;
  countryCode?: string;
  countryName?: string;
  coAddress?: string;
  age: string;
  birthdate: Date;
  deceased: boolean;
  disenfranchised: boolean;
  exemptDigitalPost: boolean;
  lastValidationDate?: Date;
}
