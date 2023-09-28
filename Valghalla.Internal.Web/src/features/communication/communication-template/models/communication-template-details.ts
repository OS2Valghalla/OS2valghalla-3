import { FileReference } from "src/shared/models/file-storage/file-reference";

export interface CommunicationTemplateDetails {
    id: string;
    title: string;
    subject?: string;
    content?: string;
    templateType: number;
    isDefaultTemplate?: boolean;
    communicationTemplateFileReferences: FileReference[];
}