namespace Valghalla.Database.Entities.Views;

public partial class ApplicationView
{
    public Guid? PersonId { get; set; }

    public Guid FormPostId { get; set; }

    public string? Bid { get; set; }

    public DateTime FormPostCreated { get; set; }

    public string? FormPostStatus { get; set; }

    public string? FormPostMotivation { get; set; }

    public Guid? FormId { get; set; }

    public Guid? ElectionId { get; set; }

    public string? FormName { get; set; }

    public int? FormType { get; set; }

    public Guid? FormFieldId { get; set; }

    public int? RowIndex { get; set; }

    public string? FieldName { get; set; }

    public string? FieldType { get; set; }

    public Guid? FieldDataId { get; set; }

    public string? FieldDataData { get; set; }

    public int? FieldDataIndex { get; set; }

    public string? PersonSocialSecurityNumber { get; set; }

    public string? PersonFirstName { get; set; }

    public string? PersonLastName { get; set; }

    public string? PersonEmail { get; set; }

    public string? PersonPhoneNumber { get; set; }

    public string? PersonMobilePhone { get; set; }

    public string? PersonAddress { get; set; }

    public string? PersonCo { get; set; }

    public string? PersonCar { get; set; }

    public string? PersonPostalPlace { get; set; }

    public string? PersonPostalCode { get; set; }

    public string? PersonStatus { get; set; }

    public string? PersonStatusId { get; set; }

    public string? PersonGender { get; set; }

    public int? PersonAge { get; set; }

    public string? PersonType { get; set; }

    public Guid? PersonTypeId { get; set; }

    public string? PersonFreeText { get; set; }

    public string? PersonTag { get; set; }

    public DateTime? ApplicationOfInterestCreated { get; set; }

}
