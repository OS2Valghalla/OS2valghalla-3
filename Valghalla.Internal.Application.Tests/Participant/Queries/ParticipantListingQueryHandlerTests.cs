using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Participant.Queries;
using Valghalla.Internal.Application.Modules.Participant.Responses;

namespace Valghalla.Internal.Application.Tests.Participant.Queries
{
    [TestClass]
    public class ParticipantListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<ParticipantListingQueryForm, ParticipantListingItemResponse, ParticipantListingQueryFormParameters> _mockQueryEngineRepository;

        public ParticipantListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<ParticipantListingQueryForm, ParticipantListingItemResponse, ParticipantListingQueryFormParameters>>();
        }

        [TestMethod]
        public async Task ParticipantListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new ParticipantListingQueryForm();
            var handler = new QueryEngineHandler<ParticipantListingQueryForm, ParticipantListingItemResponse, ParticipantListingQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }
    }
}
