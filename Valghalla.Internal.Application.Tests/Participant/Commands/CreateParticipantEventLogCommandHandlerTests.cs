using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Tests.Participant.Commands
{
    [TestClass]
    public class CreateParticipantEventLogCommandHandlerTests
    {
        private readonly IParticipantEventLogCommandRepository _mockParticipantEventLogCommandRepository;

        public CreateParticipantEventLogCommandHandlerTests()
        {
            _mockParticipantEventLogCommandRepository = Substitute.For<IParticipantEventLogCommandRepository>();
        }

        [TestMethod]
        public async Task CreateParticipantEventLogCommandHandler_Should_CallCreateParticipantEventLogAsyncOnRepository()
        {
            var command = new CreateParticipantEventLogCommand()
            {
                ParticipantId = Guid.NewGuid(),
                Text = "Hello, World!"
            };

            var handler = new CreateParticipantEventLogCommandHandler(_mockParticipantEventLogCommandRepository);


            await handler.Handle(command, default);

            await _mockParticipantEventLogCommandRepository.Received(1).CreateParticipantEventLogAsync(command, default);
        }

        [TestMethod]
        public void CreateParticipantEventLogCommandHandler_Should_ReturnValidationErrorOnEmptyParticipantId()
        {
            var command = new CreateParticipantEventLogCommand()
            {
                ParticipantId = Guid.Empty,
                Text = "Hello, World!"
            };

            var validator = new CreateParticipantEventLogCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ParticipantId);
        }

        [TestMethod]
        public void CreateParticipantEventLogCommandHandler_Should_ReturnValidationErrorOnEmptyText()
        {
            var command = new CreateParticipantEventLogCommand()
            {
                ParticipantId = Guid.NewGuid(),
                Text = string.Empty
            };

            var validator = new CreateParticipantEventLogCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Text);
        }
    }
}
