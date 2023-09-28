namespace Valghalla.Database.Entities.Tables
{
    public partial class WorkLocationTeamEntity
    {
        public Guid WorkLocationId { get; set; }
        public Guid TeamId { get; set; }
        public virtual WorkLocationEntity WorkLocation { get; set; } = null!;
        public virtual TeamEntity Team { get; set; } = null!;
        public virtual ICollection<TaskAssignmentEntity> TaskAssignments { get; } = new List<TaskAssignmentEntity>();
    }
}
