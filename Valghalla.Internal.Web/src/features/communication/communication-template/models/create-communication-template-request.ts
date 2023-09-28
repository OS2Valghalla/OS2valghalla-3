export interface CreateCommunicationTemplateRequest {
    title: string;
    subject?: string;
    content?: string;
    templateType: number;
    fileReferenceIds?: string[];
}