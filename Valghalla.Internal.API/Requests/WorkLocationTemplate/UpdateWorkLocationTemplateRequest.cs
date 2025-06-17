namespace Valghalla.Internal.API.Requests.Administration.WorkLocationTemplate
{
    public sealed record UpdateWorkLocationTemplateRequest
    {
        public Guid Id { get; init; }
        public Guid ElectionId { get; init; }
        public string Title { get; init; } = string.Empty;
        public Guid AreaId { get; init; }
        public string Address { get; init; }
        public string PostalCode { get; init; }
        public string City { get; init; }
        public int VoteLocation { get; set; }
        public List<Guid> TaskTypeIds { get; init; } = new List<Guid>();
        public List<Guid> TeamIds { get; init; } = new List<Guid>();
        public List<Guid> ResponsibleIds { get; init; } = new List<Guid>();
    }
}
