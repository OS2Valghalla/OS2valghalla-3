using Valghalla.Database.Interceptors.Audit;
using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class TaskLinkEntity : IElectionConnectedEntity
    {
        public Guid Id { get; set; }
        public Guid ElectionId { get; set; }
        
        public string? HashValue { get; set; }
        public string? Value { get; set; }

        public virtual ElectionEntity Election { get; set; } = null!;
    }
    public partial class TasksFilteredLinkEntity : IElectionConnectedEntity
    {
        public Guid Id { get; set; }
        public Guid ElectionId { get; set; }
        public ElectionEntity? Election { get; set; }
        public string? HashValue { get; set; }
        public string? Value { get; set; }

    }
    public partial class TeamLinkEntity
    {
        public Guid Id { get; set; }        
        public string? HashValue { get; set; }
        public string? Value { get; set; }
    }
}
