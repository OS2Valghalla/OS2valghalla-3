using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Commands;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.TaskType.Commands
{
    [TestClass]
    public class DeleteTaskTypeCommandHandlerTests
    {
        private readonly ITaskTypeCommandRepository _mockCommandRepository;
        private readonly ITaskTypeQueryRepository _mockQueryRepository;

        public DeleteTaskTypeCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ITaskTypeCommandRepository>();
            _mockQueryRepository = Substitute.For<ITaskTypeQueryRepository>();
        }

        [TestMethod]
        public async Task DeleteTaskTypeCommandHandler_Should_CallDeleteTaskTypeAsyncOnRepository()
        {
            Guid id = Guid.NewGuid();
            var command = new DeleteTaskTypeCommand(id);
            var handler = new DeleteTaskTypeCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DeleteTaskTypeAsync(command, default);
        }

        [TestMethod]
        public void DeleteTaskTypeCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteTaskTypeCommand(Guid.Empty);
            var validator = new DeleteTaskTypeCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
