using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Participant.Queries;

namespace Valghalla.Internal.Application.Tests.Participant.Queries
{
    [TestClass]
    public class GetWorkLocationResponsibleRightsQueryHandlerTests
    {
        private readonly IParticipantQueryRepository _mockParticipantQueryRepository;

        public GetWorkLocationResponsibleRightsQueryHandlerTests()
        {
            _mockParticipantQueryRepository = Substitute.For<IParticipantQueryRepository>();
        }

        [TestMethod]
        public async Task GetWorkLocationResponsibleRightsQueryHandler_Should_CallGetWorkLocationResponsibleRightsAsyncOnRepository()
        {
            var participantId = Guid.NewGuid();
            var query = new GetWorkLocationResponsibleRightsQuery(participantId);
            var handler = new GetWorkLocationResponsibleRightsQueryHandler(_mockParticipantQueryRepository);

            await handler.Handle(query, default);

            await _mockParticipantQueryRepository.Received(1).GetWorkLocationResponsibleRightsAsync(query, default);
        }

        [TestMethod]
        public void GetWorkLocationResponsibleRightsQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var query = new GetWorkLocationResponsibleRightsQuery(Guid.Empty);
            var validator = new GetWorkLocationResponsibleRightsQueryValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(query => query.Id);
        }
    }
}
