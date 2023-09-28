namespace Valghalla.Database.Entities.Tables;

public partial class AuditLogEntity
{
    public Guid Id { get; set; }
    public string? Pk2name { get; set; }
    public Guid? Pk2value { get; set; }
    public string? Col2name { get; set; }
    public string? Col2value { get; set; }
    public string? Col3name { get; set; }
    public string? Col3value { get; set; }
    public Guid? DoneBy { get; set; }
    public DateTime EventDate { get; set; }
    public string EventTable { get; set; } = null!;
    public string EventType { get; set; } = null!;
    public string? EventDescription { get; set; }
    public virtual UserEntity? DoneByUser { get; set; }
}
