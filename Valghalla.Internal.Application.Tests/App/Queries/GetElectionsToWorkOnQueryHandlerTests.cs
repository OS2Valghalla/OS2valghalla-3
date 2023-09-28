using NSubstitute;
using Valghalla.Internal.Application.Modules.App.Interfaces;
using Valghalla.Internal.Application.Modules.App.Queries;

namespace Valghalla.Internal.Application.Tests.App.Queries
{

    // Changed to NSubstitute instead of Moq!
    [TestClass]
    public class GetElectionsToWorkOnQueryHandlerTests
    {
        private readonly IAppElectionQueryRepository _mockAppElectionQueryRepository;

        public GetElectionsToWorkOnQueryHandlerTests()
        {
            _mockAppElectionQueryRepository = Substitute.For<IAppElectionQueryRepository>();
        }

        [TestMethod]
        public async Task GetElectionsToWorkOnQueryHandler_Should_CallGetElectionsToWorkOnAsyncOnRepository()
        {
            var query = new GetElectionsToWorkOnQuery();
            var handler = new GetElectionsToWorkOnQueryHandler(_mockAppElectionQueryRepository);

            await handler.Handle(query, default);

            await _mockAppElectionQueryRepository.Received(1).GetElectionsToWorkOnAsync(query, default);
        }
    }
}
