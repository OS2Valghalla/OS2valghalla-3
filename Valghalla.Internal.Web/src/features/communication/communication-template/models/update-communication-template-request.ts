export interface UpdateCommunicationTemplateRequest {
    id: string;
    title: string;
    subject?: string;
    content?: string;
    templateType: number;
    fileReferenceIds?: string[];
}