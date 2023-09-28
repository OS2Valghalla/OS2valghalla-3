using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Tests.Participant.Commands
{
    [TestClass]
    public class BulkDeleteParticipantsCommandHandlerTests
    {
        private readonly IParticipantGdprCommandRepository _mockParticipantGdprCommandRepository;

        public BulkDeleteParticipantsCommandHandlerTests()
        {
            _mockParticipantGdprCommandRepository = Substitute.For<IParticipantGdprCommandRepository>();
        }

        [TestMethod]
        public async Task BulkDeleteParticipantsCommandHandler_Should_CallDeleteParticipantsAsyncOnRepository()
        {
            var command = new BulkDeleteParticipantsCommand(new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            });

            var handler = new BulkDeleteParticipantsCommandHandler(_mockParticipantGdprCommandRepository, new MockQueueService());


            await handler.Handle(command, default);

            await _mockParticipantGdprCommandRepository.Received(1).DeleteParticipantsAsync(command, default);
        }

        [TestMethod]
        public void BulkDeleteParticipantsCommandHandler_Should_ReturnValidationErrorOnEmptyParticipantIds()
        {
            var command = new BulkDeleteParticipantsCommand(Array.Empty<Guid>());
            var validator = new BulkDeleteParticipantsCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ParticipantIds);
        }
    }
}
