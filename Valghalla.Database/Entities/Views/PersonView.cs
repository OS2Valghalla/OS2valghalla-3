namespace Valghalla.Database.Entities.Views;

public partial class PersonView
{
    public string? Temp { get; set; }

    public Guid PersonId { get; set; }

    public Guid ElectionId { get; set; }

    public string PersonSocialSecurityNumber { get; set; } = null!;

    public int? Length { get; set; }

    public string? PersonGenderId { get; set; }

    public string? PersonGender { get; set; }

    public int? PersonAge { get; set; }

    public string? PersonFirstName { get; set; }

    public string? PersonLastName { get; set; }

    public string? PersonPhoneNumber { get; set; }

    public string? PersonMobilePhone { get; set; }

    public string? PersonEmail { get; set; }

    public string? PersonAddress { get; set; }

    public string? PersonPostalCode { get; set; }

    public string? PersonPostalPlace { get; set; }

    public string? PersonFreeText { get; set; }

    public string? PersonCar { get; set; }

    public string? PersonStatus { get; set; }

    public string? PersonStatusId { get; set; }

    public string? PersonType { get; set; }

    public Guid? PersonTypeId { get; set; }

    public string? CourseOccassionCourseInfo { get; set; }

    public string? CourseOccassionCourseDescription { get; set; }

    public Guid? CourseOccassionCourseResponsibleId { get; set; }

    public string? CourseOccassionHall { get; set; }

    public string? CourseOccassionAddress { get; set; }

    public TimeSpan? CourseOccassionEndTime { get; set; }

    public TimeSpan? CourseOccassionStartTime { get; set; }

    public DateTime? CourseOccassionDate { get; set; }

    public string? CourseOccassionRoom { get; set; }

    public string? AssignmentFunction { get; set; }

    public string? AssignmentPublicFunction { get; set; }

    public string? GroupName { get; set; }

    public string? ConstituencyName { get; set; }

    public string? ParishName { get; set; }

    public string? ElectoralDistrictName { get; set; }

    public string? PersonCo { get; set; }

    public string? FeeType { get; set; }

    public int? Fee { get; set; }

    public double? MileCompensation { get; set; }

    public double? Expense { get; set; }

    public string? Accepted { get; set; }

    public string? PersonTag { get; set; }

    public DateTime? ApplicationOfInterestCreated { get; set; }
}
