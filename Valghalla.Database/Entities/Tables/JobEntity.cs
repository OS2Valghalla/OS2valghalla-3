namespace Valghalla.Database.Entities.Tables
{
    public partial class JobEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid? JobId { get; set; }
    }
}
