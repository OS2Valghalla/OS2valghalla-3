namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses
{
    public record WorkLocationDetailResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public Guid AreaId { get; init; }
        public string AreaName { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string PostalCode { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public int VoteLocation { get; set; }
        public bool HasActiveElection { get; set; } = false;
        public List<Guid> TaskTypeIds { get; init; } = new List<Guid>();
        public List<Guid> TeamIds { get; init; } = new List<Guid>();
        public List<Guid> ResponsibleIds { get; init; } = new List<Guid>();
    }
}
