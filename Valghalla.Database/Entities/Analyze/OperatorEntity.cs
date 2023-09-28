namespace Valghalla.Database.Entities.Analyze;

public partial class OperatorEntity
{
    public int OperatorId { get; set; }

    public string? Name { get; set; }

    public string Symbol { get; set; } = null!;

    public virtual ICollection<ColumnOperatorEntity> ColumnOperators { get; } = new List<ColumnOperatorEntity>();
}
