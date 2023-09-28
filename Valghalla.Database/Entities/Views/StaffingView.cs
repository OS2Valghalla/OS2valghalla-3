namespace Valghalla.Database.Entities.Views;

public partial class StaffingView
{
    public string? Description { get; set; }

    public string? PublicDescription { get; set; }

    public string? PersonSocialSecurityNumber { get; set; }

    public string? PersonFirstName { get; set; }

    public string? PersonLastName { get; set; }

    public string? PersonPhoneNumber { get; set; }

    public string? PersonMobilePhone { get; set; }

    public string? PersonEmail { get; set; }

    public string? PersonAddress { get; set; }

    public string? PersonPostalCode { get; set; }

    public string? PersonPostPlace { get; set; }

    public string? PersonFreeText { get; set; }

    public string? PersonStatus { get; set; }

    public string? PersonStatusId { get; set; }

    public string? PersonType { get; set; }

    public bool Accepted { get; set; }

    public Guid? ElectoralDistrictId { get; set; }

    public Guid? GroupId { get; set; }

    public Guid? PersonId { get; set; }

    public Guid? ElectionId { get; set; }

    public string? PersonCo { get; set; }
}
