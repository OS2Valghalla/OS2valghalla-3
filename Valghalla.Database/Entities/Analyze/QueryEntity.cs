namespace Valghalla.Database.Entities.Analyze;

public partial class QueryEntity
{
    public int QueryId { get; set; }

    public int ListTypeId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsTemplate { get; set; }

    public string SystemName { get; set; } = null!;

    public bool IsGlobal { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual ICollection<FilterEntity> Filters { get; } = new List<FilterEntity>();

    public virtual ICollection<ResultColumnEntity> ResultColumns { get; } = new List<ResultColumnEntity>();

    public virtual ICollection<SortColumnEntity> SortColumns { get; } = new List<SortColumnEntity>();
}
