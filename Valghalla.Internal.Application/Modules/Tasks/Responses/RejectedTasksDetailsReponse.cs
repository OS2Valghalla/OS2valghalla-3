namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record RejectedTasksDetailsReponse
    {
        public string ParticipantName { get; set; }
        public string TaskDate { get; set; }
        public string AreaName { get; set; }
        public string TeamName { get; set; }
        public string WorkLocationName { get; set; }
        public string TaskTypeName { get; set; }
        public string RejectedDate { get; set; }
    }
}
