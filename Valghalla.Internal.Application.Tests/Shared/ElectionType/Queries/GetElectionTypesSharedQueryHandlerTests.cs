using NSubstitute;
using Valghalla.Internal.Application.Modules.Shared.ElectionType.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.ElectionType.Queries;

namespace Valghalla.Internal.Application.Tests.Shared.ElectionType.Queries
{
    [TestClass]
    public class GetElectionTypesSharedQueryHandlerTests
    {
        private readonly IElectionTypeSharedQueryRepository _mockElectionTypeSharedQueryRepository;

        public GetElectionTypesSharedQueryHandlerTests()
        {
            _mockElectionTypeSharedQueryRepository = Substitute.For<IElectionTypeSharedQueryRepository>();
        }

        [TestMethod]
        public async Task GetElectionTypesSharedQueryHandler_Should_CallGetElectionTypesAsyncOnRepository()
        {
            var query = new GetElectionTypesSharedQuery();
            var handler = new GetElectionTypesSharedQueryHandler(_mockElectionTypeSharedQueryRepository);

            await handler.Handle(query, default);

            await _mockElectionTypeSharedQueryRepository.Received(1).GetElectionTypesAsync(query, default);
        }
    }
}
