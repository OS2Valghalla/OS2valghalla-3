namespace Valghalla.Database.Entities.Views;

public partial class AssignmentView
{
    public Guid? PersonId { get; set; }

    public Guid ElectionId { get; set; }

    public string? AssignementFunction { get; set; }

    public string? AssignementPublicFunction { get; set; }

    public string? GroupName { get; set; }

    public string? ConstituencyName { get; set; }

    public string? ParishName { get; set; }

    public string? ElectoralDistrictName { get; set; }

    public string FeeType { get; set; } = null!;

    public int? Fee { get; set; }

    public double? MileCompensation { get; set; }

    public double? Expense { get; set; }

    public string Accepted { get; set; } = null!;

    public Guid? ElectoralDistrictId { get; set; }

    public Guid? GroupId { get; set; }
}
