using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.Communication;
using Valghalla.Internal.Application.Modules.Tasks.Commands;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;
using Valghalla.Internal.Application.Modules.Tasks.Responses;

namespace Valghalla.Internal.Application.Tests.Tasks.Commands
{
    [TestClass]
    public class RemoveParticipantFromTaskCommandHandlerTests
    {
        private readonly ICommunicationService _mockCommunicationService;
        private readonly IElectionWorkLocationTasksCommandRepository _mockCommandRepository;
        private readonly IElectionWorkLocationTasksQueryRepository _mockQueryRepository;

        public RemoveParticipantFromTaskCommandHandlerTests()
        {
            _mockCommunicationService = Substitute.For<ICommunicationService>();
            _mockCommandRepository = Substitute.For<IElectionWorkLocationTasksCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionWorkLocationTasksQueryRepository>();
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

            _mockQueryRepository.GetTaskAssignmentAsync(default, default).ReturnsForAnyArgs(new TaskAssignmentResponse()
            {
                ParticipantId = Guid.NewGuid(),
                Accepted = false,
                Responsed = false
            });

            var handler = new RemoveParticipantFromTaskCommandHandler(_mockCommunicationService, _mockCommandRepository, _mockQueryRepository);

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
