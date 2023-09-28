namespace Valghalla.Database.Entities.Analyze;

public partial class ColumnOperatorEntity
{
    public int ColumnOperatorId { get; set; }

    public int ColumnId { get; set; }

    public int OperatorId { get; set; }

    public virtual ColumnEntity Column { get; set; } = null!;

    public virtual ICollection<FilterColumnEntity> FilterColumns { get; } = new List<FilterColumnEntity>();

    public virtual OperatorEntity Operator { get; set; } = null!;
}
