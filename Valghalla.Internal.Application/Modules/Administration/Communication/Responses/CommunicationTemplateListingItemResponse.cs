﻿namespace Valghalla.Internal.Application.Modules.Administration.Communication.Responses
{
    public sealed record CommunicationTemplateListingItemResponse
    {
        public Guid Id { get; init; }
        public string Title { get; set; } = null!;
        public string? Subject { get; set; }
        public int TemplateType { get; set; }
        public bool? IsDefaultTemplate { get; set; }
    }
}
