using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Shared.Participant.Queries;
using Valghalla.Internal.Application.Modules.Shared.Participant.Responses;

namespace Valghalla.Internal.Application.Tests.Shared.Participant.Queries
{
    [TestClass]
    public class ParticipantSharedListingQueryFormHandlerTests
    {
        private readonly IQueryEngineRepository<ParticipantSharedListingQueryForm, ParticipantSharedListingItemResponse, ParticipantSharedListingQueryFormParameters> _mockQueryEngineRepository;

        public ParticipantSharedListingQueryFormHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<ParticipantSharedListingQueryForm, ParticipantSharedListingItemResponse, ParticipantSharedListingQueryFormParameters>>();
        }

        [TestMethod]
        public async Task ParticipantSharedListingQueryFormHandler_Should_CallGetQueryFormInfoOnRepository()
        {
            var query = new ParticipantSharedListingQueryFormParameters();
            var handler = new QueryEngineFormHandler<ParticipantSharedListingQueryForm, ParticipantSharedListingItemResponse, ParticipantSharedListingQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).GetQueryFormInfo(query, default);
        }
    }
}
