using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.App.Interfaces;
using Valghalla.Internal.Application.Modules.App.Queries;

namespace Valghalla.Internal.Application.Tests.App.Queries
{
    [TestClass]
    public class GetInternalUserQueryHandlerTests
    {
        private readonly IAppUserQueryRepository _mockAppUserQueryRepository;

        public GetInternalUserQueryHandlerTests()
        {
            _mockAppUserQueryRepository = Substitute.For<IAppUserQueryRepository>();
        }

        [TestMethod]
        public async Task GetInternalUserQueryHandler_Should_CallGetUserAsyncOnRepository()
        {
            var query = new GetInternalUserQuery("1208659999", "23132323");

            var handler = new GetInternalUserQueryHandler(
                _mockAppUserQueryRepository,
                new MockAppMemoryCache());

            await handler.Handle(query, default);

            await _mockAppUserQueryRepository.Received(1).GetUserAsync(query, default);
        }

        [TestMethod]
        public void GetInternalUserQueryHandler_Should_ReturnValidationErrorOnEmptyCvrNumber()
        {
            var query = new GetInternalUserQuery(string.Empty, "3213213");
            var validator = new GetInternalUserQueryValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(query => query.CvrNumber);
        }

        [TestMethod]
        public void GetInternalUserQueryHandler_Should_ReturnValidationErrorOnEmptySerial()
        {
            var query = new GetInternalUserQuery("1208659999", string.Empty);
            var validator = new GetInternalUserQueryValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(query => query.Serial);
        }
    }
}
