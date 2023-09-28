import { FileReference } from "src/shared/models/file-storage/file-reference";

export interface ContactInformation {        
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