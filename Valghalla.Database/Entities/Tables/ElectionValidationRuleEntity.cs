namespace Valghalla.Database.Entities.Tables;

public partial class ElectionValidationRuleEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public virtual ICollection<ElectionTypeValidationRuleEntity> ValidationRules { get; } = Array.Empty<ElectionTypeValidationRuleEntity>();
}
