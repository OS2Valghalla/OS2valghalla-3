using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

namespace Valghalla.Internal.Application.Tests.Tasks.Queries
{
    [TestClass]
    public class GetTaskAssigmentQueryHandlerTests
    {
        private readonly IElectionWorkLocationTasksQueryRepository _mockQueryRepository;

        public GetTaskAssigmentQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IElectionWorkLocationTasksQueryRepository>();
        }

        [TestMethod]
        public async Task GetTaskAssigmentQueryHandler_Should_CallOnRepository()
        {
            var query = new GetTaskAssignmentQuery(Guid.NewGuid(), Guid.NewGuid());
            var handler = new GetTaskAssignmentQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetTaskAssignmentAsync(query, default);
        }

        [TestMethod]
        public void GetTaskAssigmentQueryHandler_Should_ReturnValidationErrorOnEmptyTaskAssignmentId()
        {
            var command = new GetTaskAssignmentQuery(Guid.Empty, Guid.NewGuid());
            var validator = new GetTaskAssignmentQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.TaskAssignmentId);
        }

        [TestMethod]
        public void GetTaskAssigmentQueryHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new GetTaskAssignmentQuery(Guid.NewGuid(), Guid.Empty);
            var validator = new GetTaskAssignmentQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }
    }
}
