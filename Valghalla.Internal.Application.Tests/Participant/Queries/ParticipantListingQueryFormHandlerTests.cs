using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Participant.Queries;
using Valghalla.Internal.Application.Modules.Participant.Responses;

namespace Valghalla.Internal.Application.Tests.Participant.Queries
{
    [TestClass]
    public class ParticipantListingQueryFormHandlerTests
    {
        private readonly IQueryEngineRepository<ParticipantListingQueryForm, ParticipantListingItemResponse, ParticipantListingQueryFormParameters> _mockQueryEngineRepository;

        public ParticipantListingQueryFormHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<ParticipantListingQueryForm, ParticipantListingItemResponse, ParticipantListingQueryFormParameters>>();
        }

        [TestMethod]
        public async Task ParticipantListingQueryFormHandler_Should_CallGetQueryFormInfoOnRepository()
        {
            var query = new ParticipantListingQueryFormParameters();
            var handler = new QueryEngineFormHandler<ParticipantListingQueryForm, ParticipantListingItemResponse, ParticipantListingQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).GetQueryFormInfo(query, default);
        }
    }
}
