using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Tasks.Commands;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.Application.Tests.Tasks.Commands
{
    [TestClass]
    public class RemoveParticipantFromTaskCommandHandlerTests
    {
        private readonly IElectionWorkLocationTasksCommandRepository _mockCommandRepository;

        public RemoveParticipantFromTaskCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionWorkLocationTasksCommandRepository>();
        }

        [TestMethod]
        public async Task RemoveParticipantFromTaskCommandHandlerTests_Should_CallRemoveParticipantFromTaskAsyncOnRepository()
        {
            var request = new RemoveParticipantFromTaskRequest()
            {
                ElectionId = Guid.NewGuid(),
                TaskAssignmentId = Guid.NewGuid(),
            };
            var command = new RemoveParticipantFromTaskCommand()
            {
                ElectionId = request.ElectionId,
                TaskAssignmentId = request.TaskAssignmentId
            };
            var handler = new RemoveParticipantFromTaskCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).RemoveParticipantFromTaskAsync(command, default);
        }

        [TestMethod]
        public void RemoveParticipantFromTaskCommandHandlerTests_Should_ReturnValidationErrorOnEmptyTaskAssignmentId()
        {
            var request = new RemoveParticipantFromTaskRequest()
            {
                ElectionId = Guid.NewGuid(),
            };
            var command = new RemoveParticipantFromTaskCommand()
            {
                ElectionId = request.ElectionId
            };
            var validator = new RemoveParticipantFromTaskCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.TaskAssignmentId);
        }

        [TestMethod]
        public void RemoveParticipantFromTaskCommandHandlerTests_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var request = new RemoveParticipantFromTaskRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
            };
            var command = new RemoveParticipantFromTaskCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId
            };
            var validator = new RemoveParticipantFromTaskCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }
    }
}
