namespace Valghalla.Database.Entities.Tables
{
    public partial class WorkLocationTaskTypeEntity
    {
        public Guid WorkLocationId { get; set; }
        public Guid TaskTypeId { get; set; }
        public virtual WorkLocationEntity WorkLocation { get; set; } = null!;
        public virtual TaskTypeEntity TaskType { get; set; } = null!;
        public virtual ICollection<TaskAssignmentEntity> TaskAssignments { get; } = new List<TaskAssignmentEntity>();
    }
}
