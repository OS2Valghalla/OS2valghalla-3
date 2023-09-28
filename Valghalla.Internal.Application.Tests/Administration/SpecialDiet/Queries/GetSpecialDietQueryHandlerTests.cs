using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.SpecialDiet.Queries
{
    [TestClass]
    public class GetSpecialDietQueryHandlerTests
    {
        private readonly ISpecialDietQueryRepository _mockQueryRepository;

        public GetSpecialDietQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<ISpecialDietQueryRepository>();
        }

        [TestMethod]
        public async Task GetSpecialDietQueryHandler_Should_CallOnRepository()
        {
            var query = new GetSpecialDietQuery(Guid.NewGuid());
            var handler = new GetSpecialDietQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetSpecialDietAsync(query, default);
        }

        [TestMethod]
        public void GetSpecialDietQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new GetSpecialDietQuery(Guid.Empty);
            var validator = new GetSpecialDietQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.SpecialDietId);
        }
    }
}
