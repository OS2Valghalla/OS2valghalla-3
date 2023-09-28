namespace Valghalla.Database.Entities.Analyze;

public partial class ListTypeColumnEntity
{
    public int ColumnId { get; set; }

    public int ListTypeId { get; set; }

    public int Ordinal { get; set; }

    public int RelatedListTypeId { get; set; }

    public virtual ColumnEntity Column { get; set; } = null!;

    public virtual ListTypeEntity ListType { get; set; } = null!;
}
