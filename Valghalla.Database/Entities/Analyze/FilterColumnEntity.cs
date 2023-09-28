namespace Valghalla.Database.Entities.Analyze;

public partial class FilterColumnEntity
{
    public int FilterColumnId { get; set; }

    public int FilterId { get; set; }

    public int ColumnId { get; set; }

    public int ColumnOperatorId { get; set; }

    public virtual ColumnEntity Column { get; set; } = null!;

    public virtual ColumnOperatorEntity ColumnOperator { get; set; } = null!;

    public virtual FilterEntity Filter { get; set; } = null!;

    public virtual ICollection<FilterColumnValueEntity> FilterColumnValues { get; } = new List<FilterColumnValueEntity>();
}
