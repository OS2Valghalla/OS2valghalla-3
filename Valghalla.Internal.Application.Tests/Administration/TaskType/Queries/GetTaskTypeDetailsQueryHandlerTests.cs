using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.TaskType.Queries
{
    [TestClass]
    public class GetTaskTypeDetailsQueryHandlerTests
    {
        private readonly ITaskTypeQueryRepository _mockQueryRepository;

        public GetTaskTypeDetailsQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<ITaskTypeQueryRepository>();
        }

        [TestMethod]
        public async Task GetTaskTypeDetailsQueryHandler_Should_CallOnRepository()
        {
            var query = new GetTaskTypeDetailsQuery(Guid.NewGuid());
            var handler = new GetTaskTypeDetailsQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetTaskTypeAsync(query.Id, default);
        }

        [TestMethod]
        public void GetTaskTypeDetailsQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new GetTaskTypeDetailsQuery(Guid.Empty);
            var validator = new GetTaskTypeDetailsQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
