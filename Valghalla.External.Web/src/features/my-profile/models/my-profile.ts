export interface MyProfile {
  participantId: string;
  cpr: string;
  firstName?: string;
  lastName?: string;
  streetAddress?: string;
  postalCode?: string;
  city?: string;
  municipalityName?: string;
  mobileNumber?: string;
  email?: string;
  specialDietIds: string[];
}
