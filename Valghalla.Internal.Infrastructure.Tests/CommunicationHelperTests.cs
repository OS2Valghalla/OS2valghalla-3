using NSubstitute;

using Valghalla.Application.Communication;
using Valghalla.Application.Tenant;
using Valghalla.Integration.Communication;

namespace Valghalla.Internal.Infrastructure.Tests;

[TestClass]
public class CommunicationHelperTests
{
    //private ICommunicationQueryRepository mockQueryRepo;
    //private ITenantContextProvider mockTenantContext;
    //private CommunicationHelper helper; // Change access modifier to public for testing
    //private readonly Guid participantId = Guid.Parse("e0d55398-56e5-4abe-bc71-dcc386f23f77");
    //private readonly CancellationToken token = CancellationToken.None;

    //[TestInitialize]
    //public void Setup()
    //{
    //    mockQueryRepo = Substitute.For<ICommunicationQueryRepository>();
    //    mockTenantContext = Substitute.For<ITenantContextProvider>();
    //    helper = new CommunicationHelper(mockTenantContext, mockQueryRepo);
    //}

    //[TestMethod]
    //public async Task ValidateTaskReminderParticipantAsync_ReturnsFalse_WhenDigitalPostAndExempt()
    //{
    //    // Arrange
    //    var participant = new CommunicationParticipantInfo
    //    {
    //        Id = Guid.Parse("e0d55398-56e5-4abe-bc71-dcc386f23f77"),
    //        ExemptDigitalPost = true,
    //        Email = "test@example.com",
    //        MobileNumber = "12345678"
    //    };

    //    mockQueryRepo.GetParticipantAsync(participantId, token)
    //                 .Returns(participant);

    //    // Act
    //    var result = await helper.ValidateTaskReminderParticipantAsync(participantId, TemplateType.DigitalPost, token);

    //    // Assert
    //    Assert.IsFalse(result);
    //}

    //[TestMethod]
    //public async Task ValidateTaskReminderParticipantAsync_ReturnsFalse_WhenEmailAndMissingEmail()
    //{
    //    var participant = new CommunicationParticipantInfo
    //    {
    //        ExemptDigitalPost = false,
    //        Email = null,
    //        MobileNumber = "12345678"
    //    };

    //    mockQueryRepo.GetParticipantAsync(participantId, token)
    //                 .Returns(participant);

    //    var result = await helper.ValidateTaskReminderParticipantAsync(participantId, TemplateType.Email, token);

    //    Assert.IsFalse(result);
    //}

    //[TestMethod]
    //public async Task ValidateTaskReminderParticipantAsync_ReturnsFalse_WhenSmsAndMissingMobile()
    //{
    //    var participant = new CommunicationParticipantInfo
    //    {
    //        ExemptDigitalPost = false,
    //        Email = "test@example.com",
    //        MobileNumber = null
    //    };

    //    mockQueryRepo.GetParticipantAsync(participantId, token)
    //                 .Returns(participant);

    //    var result = await helper.ValidateTaskReminderParticipantAsync(participantId, TemplateType.SMS, token);

    //    Assert.IsFalse(result);
    //}

    //[TestMethod]
    //public async Task ValidateTaskReminderParticipantAsync_ReturnsTrue_WhenAllValidForEmail()
    //{
    //    var participant = new CommunicationParticipantInfo
    //    {
    //        ExemptDigitalPost = false,
    //        Email = "test@example.com",
    //        MobileNumber = "12345678"
    //    };

    //    mockQueryRepo.GetParticipantAsync(participantId, token)
    //                 .Returns(participant);

    //    var result = await helper.ValidateTaskReminderParticipantAsync(participantId, TemplateType.Email, token);

    //    Assert.IsTrue(result);
    //}
}
