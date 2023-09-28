using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Shared.Participant.Queries;
using Valghalla.Internal.Application.Modules.Shared.Participant.Responses;

namespace Valghalla.Internal.Application.Tests.Shared.Participant.Queries
{
    [TestClass]
    public class ParticipantSharedListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<ParticipantSharedListingQueryForm, ParticipantSharedListingItemResponse, ParticipantSharedListingQueryFormParameters> _mockQueryEngineRepository;

        public ParticipantSharedListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<ParticipantSharedListingQueryForm, ParticipantSharedListingItemResponse, ParticipantSharedListingQueryFormParameters>>();
        }

        [TestMethod]
        public async Task ParticipantSharedListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new ParticipantSharedListingQueryForm();
            var handler = new QueryEngineHandler<ParticipantSharedListingQueryForm, ParticipantSharedListingItemResponse, ParticipantSharedListingQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }
    }
}
