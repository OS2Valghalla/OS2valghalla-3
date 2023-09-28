using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

namespace Valghalla.Internal.Application.Tests.Tasks.Queries
{
    [TestClass]
    public class GetTasksFiltersOptionsQueryHandlerTests
    {
        private readonly IFilteredTasksQueryRepository _mockQueryRepository;

        public GetTasksFiltersOptionsQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IFilteredTasksQueryRepository>();
        }

        [TestMethod]
        public async Task GetTasksFiltersOptionsQueryHandler_Should_CallOnRepository()
        {
            var query = new GetTasksFiltersOptionsQuery(Guid.NewGuid());
            var handler = new GetTasksFiltersOptionsQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetTasksFiltersOptionsAsync(query, default);
        }

        [TestMethod]
        public void GetTasksFiltersOptionsQueryHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new GetTasksFiltersOptionsQuery(Guid.Empty);
            var validator = new GetTasksFiltersOptionsQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }
    }
}
