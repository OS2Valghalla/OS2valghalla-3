﻿namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses
{
    public sealed record WorkLocationResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public Guid AreaId { get; init; }
        public string AreaName { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string PostalCode { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string ElectionId { get; set; } = string.Empty;
        public string TemplateId { get; set; } = string.Empty;
        public string ElectionTitle { get; set; } = string.Empty;
        public string TemplateTitle { get; set; } = string.Empty;
    }
}
