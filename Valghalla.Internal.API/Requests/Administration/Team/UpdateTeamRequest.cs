namespace Valghalla.Internal.API.Requests.Administration.Team
{
    public class UpdateTeamRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public Guid ElectionId { get; set; }
        public string? Description { get; set; }
        public Guid TeamTypeId { get; set; }
        public List<Guid> ResponsibleIds { get; set; } = new List<Guid>();
    }
}
