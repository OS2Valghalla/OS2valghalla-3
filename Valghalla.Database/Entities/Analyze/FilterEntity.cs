namespace Valghalla.Database.Entities.Analyze;

public partial class FilterEntity
{
    public int FilterId { get; set; }

    public int QueryId { get; set; }

    public int Ordinal { get; set; }

    public virtual ICollection<FilterColumnEntity> FilterColumns { get; } = new List<FilterColumnEntity>();

    public virtual QueryEntity Query { get; set; } = null!;
}
