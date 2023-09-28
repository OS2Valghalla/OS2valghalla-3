export interface UpdateContactInformationRequest {        
    municipalityName: string;
    electionResponsibleApartment: string;
    address: string;
    postalCode: string;
    city: string;
    telephoneNumber?: string;
    digitalPost?: string;
    email?: string;
    logoFileReferenceId?: string;
}