namespace Valghalla.Database.Entities.Analyze;

public partial class ListPickerTypeEntity
{
    public int ListPickerTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ColumnEntity> Columns { get; } = new List<ColumnEntity>();
}
