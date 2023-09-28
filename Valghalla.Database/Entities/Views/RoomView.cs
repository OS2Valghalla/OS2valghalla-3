namespace Valghalla.Database.Entities.Views;

public partial class RoomView
{
    public Guid? BuildingId { get; set; }

    public Guid? ElectionId { get; set; }

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

    public string RoomName { get; set; } = null!;

    public Guid? RoomElectoralDistrictId { get; set; }

    public string? ElectoralDistrictNumber { get; set; }

    public string? ParishName { get; set; }

    public string? ParishNumber { get; set; }

    public string? ConstituencyName { get; set; }

    public string? ConstituencyNumber { get; set; }

    public string? BuildingPhoneNumber { get; set; }

    public string? BuildingWebpage { get; set; }

    public string? BuildingEmail { get; set; }

    public string? BuildingInfoToContactPerson { get; set; }

    public string? BuildingComment { get; set; }

    public string? BuildingDeliveryAddress { get; set; }

    public string? BuildingDeliveryPostalCode { get; set; }

    public string? BuildingDeliveryPostalPlace { get; set; }

    public string BuildingCanExpand { get; set; } = null!;

    public string BuildingInformerNeeded { get; set; } = null!;

    public Guid? RoomId { get; set; } = null!;

    public Guid? GroupId { get; set; } = null!;
}
