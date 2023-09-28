using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Team.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Team.Queries
{
    [TestClass]
    public class GetAllTeamsQueryHandlerTests
    {
        private readonly ITeamQueryRepository _mockQueryRepository;

        public GetAllTeamsQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<ITeamQueryRepository>();
        }

        [TestMethod]
        public async Task GetAllTeamsQueryHandler_Should_CallOnRepository()
        {
            var query = new GetAllTeamsQuery();
            var handler = new GetAllTeamsQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetAllTeamsAsync(default);
        }
    }
}
