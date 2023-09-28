using NSubstitute;
using Valghalla.Internal.Application.Modules.Shared.SpecialDiet.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.SpecialDiet.Queries;

namespace Valghalla.Internal.Application.Tests.Shared.SpecialDiet.Queries
{
    [TestClass]
    public class GetSpecialDietsSharedQueryHandlerTests
    {
        private readonly ISpecialDietSharedQueryRepository _mockSpecialDietSharedQueryRepository;

        public GetSpecialDietsSharedQueryHandlerTests()
        {
            _mockSpecialDietSharedQueryRepository = Substitute.For<ISpecialDietSharedQueryRepository>();
        }

        [TestMethod]
        public async Task GetSpecialDietsSharedQueryHandler_Should_CallGetSpecialDietsAsyncOnRepository()
        {
            var query = new GetSpecialDietsSharedQuery();
            var handler = new GetSpecialDietsSharedQueryHandler(_mockSpecialDietSharedQueryRepository);

            await handler.Handle(query, default);

            await _mockSpecialDietSharedQueryRepository.Received(1).GetSpecialDietsAsync(query, default);
        }
    }
}
