namespace Valghalla.Internal.API.Requests.Administration.WorkLocationTemplate
{
    public sealed record CreateWorkLocationTemplateRequest
    {
        public string Title { get; init; } = string.Empty;
        public Guid AreaId { get; init; }
        public string Address { get; init; } = string.Empty;
        public string PostalCode { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public int VoteLocation { get; set; }        
    }
}
