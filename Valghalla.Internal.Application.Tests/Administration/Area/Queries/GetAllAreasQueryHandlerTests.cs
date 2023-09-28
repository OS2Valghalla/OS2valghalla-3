using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Area.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Area.Queries
{
    [TestClass]
    public class GetAllAreasQueryHandlerTests
    {
        private readonly IAreaQueryRepository _mockQueryRepository;

        public GetAllAreasQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IAreaQueryRepository>();
        }

        [TestMethod]
        public async Task GetAllAreasQueryHandler_Should_CallOnRepository()
        {
            var query = new GetAllAreasQuery();
            var handler = new GetAllAreasQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetAllAreasAsync(query, default);
        }
    }
}
