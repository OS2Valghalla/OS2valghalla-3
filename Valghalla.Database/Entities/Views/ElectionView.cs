namespace Valghalla.Database.Entities.Views;

public partial class ElectionView
{
    public string? ElectionName { get; set; }

    public DateTime? ElectionDate { get; set; }

    public string? ConstituencyName { get; set; }

    public string? ConstituencyNumber { get; set; }

    public string? ParishName { get; set; }

    public string? ParishNumber { get; set; }

    public string? ElectoralDistrictNumber { get; set; }

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

    public string? PersonTyp { get; set; }

    public string? GruppNamn { get; set; }

    public Guid? ElectionId { get; set; }

    public Guid? ElectoralDistrictId { get; set; }
}
