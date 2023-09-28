using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Queries;

namespace Valghalla.Internal.Application.Tests.Shared.Participant.Queries
{
    [TestClass]
    public class GetParticipantsSharedQueryHandlerTests
    {
        private readonly IParticipantSharedQueryRepository _mockParticipantSharedQueryRepository;

        public GetParticipantsSharedQueryHandlerTests()
        {
            _mockParticipantSharedQueryRepository = Substitute.For<IParticipantSharedQueryRepository>();
        }

        [TestMethod]
        public async Task GetParticipantsSharedQueryHandler_Should_CallGetPariticipantsAsyncOnRepository()
        {
            var query = new GetParticipantsSharedQuery()
            {
                Values = new[] { Guid.NewGuid() }
            };

            var handler = new GetParticipantsSharedQueryHandler(_mockParticipantSharedQueryRepository);

            await handler.Handle(query, default);

            await _mockParticipantSharedQueryRepository.Received(1).GetPariticipantsAsync(query, default);
        }

        [TestMethod]
        public void GetParticipantsSharedQueryHandler_Should_ReturnValidationErrorOnEmptyValues()
        {
            var query = new GetParticipantsSharedQuery();
            var validator = new GetParticipantsSharedQueryValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(query => query.Values);
        }
    }
}
