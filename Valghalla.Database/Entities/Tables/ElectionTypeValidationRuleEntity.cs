namespace Valghalla.Database.Entities.Tables;

public partial class ElectionTypeValidationRuleEntity
{
    public Guid ElectionTypeId { get; set; }
    public Guid ValidationRuleId { get; set; }
    public virtual ElectionTypeEntity ElectionType { get; set; } = null!;
    public virtual ElectionValidationRuleEntity ValidationRule { get; set; } = null!;
}
