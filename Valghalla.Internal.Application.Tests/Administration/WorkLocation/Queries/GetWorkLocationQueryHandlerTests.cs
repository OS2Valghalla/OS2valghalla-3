using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.WorkLocation.Queries
{
    [TestClass]
    public class GetWorkLocationQueryHandlerTests
    {
        private readonly IWorkLocationQueryRepository _mockQueryRepository;

        public GetWorkLocationQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IWorkLocationQueryRepository>();
        }

        [TestMethod]
        public async Task GetWorkLocationQueryHandler_Should_CallOnRepository()
        {
            var query = new GetWorkLocationQuery(Guid.NewGuid());
            var handler = new GetWorkLocationQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetWorkLocationAsync(query, default);
        }

        [TestMethod]
        public void GetWorkLocationQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new GetWorkLocationQuery(Guid.Empty);
            var validator = new GetWorkLocationQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.WorkLocationId);
        }
    }
}
