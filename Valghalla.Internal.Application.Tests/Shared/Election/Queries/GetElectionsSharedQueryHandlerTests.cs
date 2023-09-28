using NSubstitute;
using Valghalla.Internal.Application.Modules.Shared.Election.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Election.Queries;

namespace Valghalla.Internal.Application.Tests.Shared.Election.Queries
{
    [TestClass]
    public class GetElectionsSharedQueryHandlerTests
    {
        private readonly IElectionSharedQueryRepository _mockElectionSharedQueryRepository;

        public GetElectionsSharedQueryHandlerTests()
        {
            _mockElectionSharedQueryRepository = Substitute.For<IElectionSharedQueryRepository>();
        }

        [TestMethod]
        public async Task GetElectionsSharedQueryHandler_Should_CallGetElectionsAsyncOnRepository()
        {
            var query = new GetElectionsSharedQuery();
            var handler = new GetElectionsSharedQueryHandler(_mockElectionSharedQueryRepository);

            await handler.Handle(query, default);

            await _mockElectionSharedQueryRepository.Received(1).GetElectionsAsync(query, default);
        }
    }
}
