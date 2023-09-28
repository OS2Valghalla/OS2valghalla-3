export interface CommunicationTemplateListingItem {
    id: string;
    title: string;
    subject?: string;
    templateType: number;
    templateTypeName?: string;
    isDefaultTemplate?: boolean;
}