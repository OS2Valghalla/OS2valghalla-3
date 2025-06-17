namespace Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Responses
{
    public sealed record WorkLocationTemplateResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public Guid AreaId { get; init; }
        public string AreaName { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string PostalCode { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
    }
}
