namespace Valghalla.Database.Entities.Tables;

public partial class UserEntity
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string Name { get; set; } = null!;
    public string? Cvr { get; set; }
    public string? Cpr { get; set; }
    public string? Serial { get; set; }

    public virtual ParticipantEntity? Participant { get; set; }
}
