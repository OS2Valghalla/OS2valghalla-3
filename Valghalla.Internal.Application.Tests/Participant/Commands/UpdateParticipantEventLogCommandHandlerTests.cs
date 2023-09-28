using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Tests.Participant.Commands
{
    [TestClass]
    public class UpdateParticipantEventLogCommandHandlerTests
    {
        private readonly IParticipantEventLogCommandRepository _mockParticipantEventLogCommandRepository;

        public UpdateParticipantEventLogCommandHandlerTests()
        {
            _mockParticipantEventLogCommandRepository = Substitute.For<IParticipantEventLogCommandRepository>();
        }

        [TestMethod]
        public async Task UpdateParticipantEventLogCommandHandler_Should_CallUpdateParticipantEventLogAsyncOnRepository()
        {
            var command = new UpdateParticipantEventLogCommand()
            {
                Id = Guid.NewGuid(),
                Text = "Hello, World!"
            };

            var handler = new UpdateParticipantEventLogCommandHandler(_mockParticipantEventLogCommandRepository);

            await handler.Handle(command, default);

            await _mockParticipantEventLogCommandRepository.Received(1).UpdateParticipantEventLogAsync(command, default);
        }

        [TestMethod]
        public void UpdateParticipantEventLogCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new UpdateParticipantEventLogCommand()
            {
                Id = Guid.Empty,
                Text = "Hello, World!"
            };

            var validator = new UpdateParticipantEventLogCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void UpdateParticipantEventLogCommandHandler_Should_ReturnValidationErrorOnEmptyText()
        {
            var command = new UpdateParticipantEventLogCommand()
            {
                Id = Guid.NewGuid(),
                Text = string.Empty
            };

            var validator = new UpdateParticipantEventLogCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Text);
        }
    }
}
