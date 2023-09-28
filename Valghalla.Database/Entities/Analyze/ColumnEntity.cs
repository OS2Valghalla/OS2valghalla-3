namespace Valghalla.Database.Entities.Analyze;

public partial class ColumnEntity
{
    public int ColumnId { get; set; }

    public int ListTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string ColumnName { get; set; } = null!;

    public int DatatypeId { get; set; }

    public bool? IsSortable { get; set; }

    public bool? IsFilterable { get; set; }

    public int? ListPickerTypeId { get; set; }

    public bool? IsViewable { get; set; }

    public int Ordinal { get; set; }

    public int RelatedListId { get; set; }

    public string? LookupFunction { get; set; }

    public int? LookupColumnId { get; set; }

    public virtual ICollection<ColumnOperatorEntity> ColumnOperators { get; } = new List<ColumnOperatorEntity>();

    public virtual DatatypeEntity Datatype { get; set; } = null!;

    public virtual ICollection<FilterColumnEntity> FilterColumns { get; } = new List<FilterColumnEntity>();

    public virtual ListPickerTypeEntity? ListPickerType { get; set; }

    public virtual ListTypeEntity ListType { get; set; } = null!;

    public virtual ICollection<ListTypeColumnEntity> ListTypeColumns { get; } = new List<ListTypeColumnEntity>();

    public virtual ICollection<ResultColumnEntity> ResultColumns { get; } = new List<ResultColumnEntity>();

    public virtual ICollection<SortColumnEntity> SortColumns { get; } = new List<SortColumnEntity>();
}
