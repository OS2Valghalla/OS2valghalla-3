using NSubstitute;
using Valghalla.Internal.Application.Modules.Shared.Area.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Area.Queries;

namespace Valghalla.Internal.Application.Tests.Shared.Area.Queries
{
    [TestClass]
    public class GetAreasSharedQueryHandlerTests
    {
        private readonly IAreaSharedQueryRepository _mockAreaSharedQueryRepository;

        public GetAreasSharedQueryHandlerTests()
        {
            _mockAreaSharedQueryRepository = Substitute.For<IAreaSharedQueryRepository>();
        }

        [TestMethod]
        public async Task GetAreasSharedQueryHandler_Should_CallGetAreasAsyncOnRepository()
        {
            var query = new GetAreasSharedQuery();
            var handler = new GetAreasSharedQueryHandler(_mockAreaSharedQueryRepository);

            await handler.Handle(query, default);

            await _mockAreaSharedQueryRepository.Received(1).GetAreasAsync(query, default);
        }
    }
}
