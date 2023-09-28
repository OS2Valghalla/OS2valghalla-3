using NSubstitute;
using Valghalla.Internal.Application.Modules.Shared.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Team.Queries;

namespace Valghalla.Internal.Application.Tests.Shared.Team.Queries
{
    [TestClass]
    public class GetTeamsSharedQueryHandlerTests
    {
        private readonly ITeamSharedQueryRepository _mockTeamSharedQueryRepository;

        public GetTeamsSharedQueryHandlerTests()
        {
            _mockTeamSharedQueryRepository = Substitute.For<ITeamSharedQueryRepository>();
        }

        [TestMethod]
        public async Task GetTeamsSharedQueryHandler_Should_CallGetTeamsAsyncOnRepository()
        {
            var query = new GetTeamsSharedQuery();
            var handler = new GetTeamsSharedQueryHandler(_mockTeamSharedQueryRepository);

            await handler.Handle(query, default);

            await _mockTeamSharedQueryRepository.Received(1).GetTeamsAsync(query, default);
        }
    }
}
