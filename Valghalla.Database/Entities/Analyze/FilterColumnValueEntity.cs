namespace Valghalla.Database.Entities.Analyze;

public partial class FilterColumnValueEntity
{
    public int FilterColumnValueId { get; set; }

    public int FilterColumnId { get; set; }

    public string ValueKey { get; set; } = null!;

    public string Value { get; set; } = null!;

    public virtual FilterColumnEntity FilterColumn { get; set; } = null!;
}
