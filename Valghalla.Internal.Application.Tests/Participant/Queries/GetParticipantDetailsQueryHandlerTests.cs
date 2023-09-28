using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.AuditLog;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Participant.Queries;

namespace Valghalla.Internal.Application.Tests.Participant.Queries
{
    [TestClass]
    public class GetParticipantDetailsQueryHandlerTests
    {
        private readonly IParticipantQueryRepository _mockParticipantQueryRepository;
        private readonly IAuditLogService _mockAuditLogService;

        public GetParticipantDetailsQueryHandlerTests()
        {
            _mockParticipantQueryRepository = Substitute.For<IParticipantQueryRepository>();
            _mockAuditLogService = Substitute.For<IAuditLogService>();
        }

        [TestMethod]
        public async Task GetParticipantDetailsQueryHandler_Should_CallGetParticipantDetailsAsyncOnRepository()
        {
            var participantId = Guid.NewGuid();
            var query = new GetParticipantDetailsQuery(participantId);
            var handler = new GetParticipantDetailsQueryHandler(_mockParticipantQueryRepository, _mockAuditLogService);

            await handler.Handle(query, default);

            await _mockParticipantQueryRepository.Received(1).GetParticipantDetailsAsync(query, default);
        }

        [TestMethod]
        public void GetParticipantDetailsQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var query = new GetParticipantDetailsQuery(Guid.Empty);
            var validator = new GetParticipantDetailsQueryValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(query => query.Id);
        }
    }
}
