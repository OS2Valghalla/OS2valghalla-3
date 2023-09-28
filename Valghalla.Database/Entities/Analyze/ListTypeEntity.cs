namespace Valghalla.Database.Entities.Analyze;

public partial class ListTypeEntity
{
    public int ListTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string View { get; set; } = null!;

    public bool IsPrimary { get; set; }

    public string? Table { get; set; }

    public virtual ICollection<ColumnEntity> Columns { get; } = new List<ColumnEntity>();

    public virtual ICollection<ListTypeColumnEntity> ListTypeColumns { get; } = new List<ListTypeColumnEntity>();

    public virtual ICollection<ListTypeEntity> PrimaryListTypes { get; } = new List<ListTypeEntity>();

    public virtual ICollection<ListTypeEntity> RelatedListTypes { get; } = new List<ListTypeEntity>();
}
