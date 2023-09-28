using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.Communication;
using Valghalla.Internal.Application.Modules.Tasks.Commands;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.Application.Tests.Tasks.Commands
{
    [TestClass]
    public class ReplyForParticipantCommandHandlerTests
    {
        private readonly IElectionWorkLocationTasksCommandRepository _mockCommandRepository;
        private readonly ICommunicationService _mockCommunicationService;

        public ReplyForParticipantCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionWorkLocationTasksCommandRepository>();
            _mockCommunicationService = Substitute.For<ICommunicationService>();
        }

        [TestMethod]
        public async Task ReplyForParticipantCommandHandlerTests_Should_CallReplyForParticipantAsyncOnRepository()
        {
            var request = new ReplyForParticipantRequest()
            {
                ElectionId = Guid.NewGuid(),
                TaskAssignmentId = Guid.NewGuid(),
                Accepted = true,
                MobileNumber = "123456",
                Email = "test@email.com",
                SpecialDietIds = new List<Guid> { Guid.NewGuid() }
            };
            var command = new ReplyForParticipantCommand()
            {
                ElectionId = request.ElectionId,
                TaskAssignmentId = request.TaskAssignmentId,
                Accepted = request.Accepted,
                MobileNumber = request.MobileNumber,
                Email = request.Email,
                SpecialDietIds = request.SpecialDietIds
            };
            var handler = new ReplyForParticipantCommandHandler(_mockCommunicationService, _mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).ReplyForParticipantAsync(command, default);
        }

        [TestMethod]
        public void ReplyForParticipantCommandHandlerTests_Should_ReturnValidationErrorOnEmptyTaskAssignmentId()
        {
            var request = new ReplyForParticipantRequest()
            {
                ElectionId = Guid.NewGuid(),
            };
            var command = new ReplyForParticipantCommand()
            {
                ElectionId = request.ElectionId
            };
            var validator = new ReplyForParticipantCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.TaskAssignmentId);
        }

        [TestMethod]
        public void ReplyForParticipantCommandHandlerTests_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var request = new ReplyForParticipantRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
            };
            var command = new ReplyForParticipantCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId
            };
            var validator = new ReplyForParticipantCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }
    }
}
