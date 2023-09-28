namespace Valghalla.Database.Entities.Analyze;

public partial class SortColumnEntity
{
    public int SortColumnId { get; set; }

    public int QueryId { get; set; }

    public int ColumnId { get; set; }

    public int Ordinal { get; set; }

    public bool IsDecending { get; set; }

    public virtual ColumnEntity Column { get; set; } = null!;

    public virtual QueryEntity Query { get; set; } = null!;
}
