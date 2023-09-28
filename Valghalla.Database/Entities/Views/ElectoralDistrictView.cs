namespace Valghalla.Database.Entities.Views;

public partial class ElectoralDistrictView
{
    public string? ConstituencyName { get; set; }

    public string? ConstituencyNumber { get; set; }

    public string ParishName { get; set; } = null!;

    public string? ParishNumber { get; set; }

    public string? ElectoralDistrictNumber { get; set; }

    public string? PersonSocialSecurityNumber { get; set; }

    public string? PersonFirstName { get; set; }

    public string? PersonLastName { get; set; }

    public string? PersonPhoneNumber { get; set; }

    public string? PersonMobilePhone { get; set; }

    public string? PersonEmail { get; set; }

    public string? PersonAddress { get; set; }

    public string? PersonPostalCode { get; set; }

    public string? PersonPostalPlace { get; set; }

    public string? PersonFreeText { get; set; }

    public string? PersonStatus { get; set; }

    public string? PersonStatusId { get; set; }

    public string? PersonType { get; set; }

    public string? RoomName { get; set; }

    public string? BuildingName { get; set; }

    public string? BuildingVisitingPostalCode { get; set; }

    public string? BuildingVisitingPostalPlace { get; set; }

    public string? BuildingVisitingAddress { get; set; }

    public Guid? BuildingDistrictId { get; set; }

    public string? BuildingDistrict { get; set; }

    public Guid? BuildingTypeId { get; set; }

    public string? BuildingType { get; set; }

    public string? BuildingStatus { get; set; }

    public string? BuildingStatusId { get; set; }

    public Guid? ElectionId { get; set; }

    public string? BuildingPhoneNumber { get; set; }

    public string? BuildingWebpage { get; set; }

    public string? BuildingEmail { get; set; }

    public string? BuildingInfoToContactPerson { get; set; }

    public string? BuildingComment { get; set; }

    public string? PersonCo { get; set; }

    public string? PersonCar { get; set; }

    public string? PersonGender { get; set; }

    public int? PersonAge { get; set; }

    public string? BuildingDeliveryAddress { get; set; }

    public string? BuildingDeliveryPostalCode { get; set; }

    public string? BuildingDeliveryPostalPlace { get; set; }

    public string? BuildingCanExpand { get; set; }

    public string? BuildingInformerNeeded { get; set; }

    public string? AssignmentFunction { get; set; }

    public string? AssignmentPublicFunction { get; set; }

    public string? GroupName { get; set; }

    public string? ElectoralDistrictName { get; set; }

    public string? FeeType { get; set; }

    public int? Fee { get; set; }

    public double? MileCompensation { get; set; }

    public double? Expense { get; set; }

    public string? Accepted { get; set; }

    public string? PersonTag { get; set; }

    public DateTime? ApplicationOfInterestCreated { get; set; }
}
