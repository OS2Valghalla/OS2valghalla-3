using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Participant.Queries;

namespace Valghalla.Internal.Application.Tests.Participant.Queries
{
    [TestClass]
    public class GetParticipantTasksQueryHandlerTests
    {
        private readonly IParticipantQueryRepository _mockParticipantQueryRepository;

        public GetParticipantTasksQueryHandlerTests()
        {
            _mockParticipantQueryRepository = Substitute.For<IParticipantQueryRepository>();
        }

        [TestMethod]
        public async Task GetParticipantTasksQueryHandler_Should_CallGetParticipantTasksAsyncOnRepository()
        {
            var participantId = Guid.NewGuid();
            var query = new GetParticipantTasksQuery(participantId);
            var handler = new GetParticipantTasksQueryHandler(_mockParticipantQueryRepository);

            await handler.Handle(query, default);

            await _mockParticipantQueryRepository.Received(1).GetParticipantTasksAsync(query, default);
        }

        [TestMethod]
        public void GetParticipantTasksQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var query = new GetParticipantTasksQuery(Guid.Empty);
            var validator = new GetParticipantTasksQueryValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(query => query.Id);
        }
    }
}
