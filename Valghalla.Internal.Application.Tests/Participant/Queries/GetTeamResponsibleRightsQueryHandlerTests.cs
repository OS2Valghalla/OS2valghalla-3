using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Participant.Queries;

namespace Valghalla.Internal.Application.Tests.Participant.Queries
{
    [TestClass]
    public class GetTeamResponsibleRightsQueryHandlerTests
    {
        private readonly IParticipantQueryRepository _mockParticipantQueryRepository;

        public GetTeamResponsibleRightsQueryHandlerTests()
        {
            _mockParticipantQueryRepository = Substitute.For<IParticipantQueryRepository>();
        }

        [TestMethod]
        public async Task GetTeamResponsibleRightsQueryHandler_Should_CallGetTeamResponsibleRightsAsyncOnRepository()
        {
            var participantId = Guid.NewGuid();
            var query = new GetTeamResponsibleRightsQuery(participantId);
            var handler = new GetTeamResponsibleRightsQueryHandler(_mockParticipantQueryRepository);

            await handler.Handle(query, default);

            await _mockParticipantQueryRepository.Received(1).GetTeamResponsibleRightsAsync(query, default);
        }

        [TestMethod]
        public void GetTeamResponsibleRightsQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var query = new GetTeamResponsibleRightsQuery(Guid.Empty);
            var validator = new GetTeamResponsibleRightsQueryValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(query => query.Id);
        }
    }
}
