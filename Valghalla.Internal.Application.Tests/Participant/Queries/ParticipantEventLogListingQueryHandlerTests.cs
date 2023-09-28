using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Participant.Queries;
using Valghalla.Internal.Application.Modules.Participant.Responses;

namespace Valghalla.Internal.Application.Tests.Participant.Queries
{
    [TestClass]
    public class ParticipantEventLogListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<ParticipantEventLogListingQueryForm, ParticipantEventLogListingItemResponse, VoidQueryFormParameters> _mockQueryEngineRepository;

        public ParticipantEventLogListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<ParticipantEventLogListingQueryForm, ParticipantEventLogListingItemResponse, VoidQueryFormParameters>>();
        }

        [TestMethod]
        public async Task ParticipantEventLogListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new ParticipantEventLogListingQueryForm() { ParticipantId = Guid.NewGuid() };
            var handler = new QueryEngineHandler<ParticipantEventLogListingQueryForm, ParticipantEventLogListingItemResponse, VoidQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }

        [TestMethod]
        public void ParticipantEventLogListingQueryHandler_Should_ReturnValidationErrorOnEmptyParticipantId()
        {
            var query = new ParticipantEventLogListingQueryForm() { ParticipantId = Guid.Empty };
            var validator = new ParticipantEventLogListingQueryFormValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(query => query.ParticipantId);
        }
    }
}
