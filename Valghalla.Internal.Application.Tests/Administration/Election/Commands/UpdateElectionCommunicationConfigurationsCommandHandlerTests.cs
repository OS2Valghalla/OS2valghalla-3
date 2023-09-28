using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.Election;
using Valghalla.Internal.Application.Modules.Administration.Election.Commands;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Election.Commands
{
    [TestClass]
    public class UpdateElectionCommunicationConfigurationsCommandHandlerTests
    {
        private readonly IElectionCommandRepository _mockCommandRepository;
        private readonly IElectionQueryRepository _mockQueryRepository;

        public UpdateElectionCommunicationConfigurationsCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionQueryRepository>();
        }

        [TestMethod]
        public async Task UpdateElectionCommunicationConfigurationsCommandHandler_Should_CallUpdateElectionCommunicationConfigurationsAsyncOnRepository()
        {
            var request = new UpdateElectionCommunicationConfigurationsRequest()
            {
                Id = Guid.NewGuid(),
                ConfirmationOfRegistrationCommunicationTemplateId = Guid.NewGuid(),
                ConfirmationOfCancellationCommunicationTemplateId = Guid.NewGuid(),
                InvitationCommunicationTemplateId = Guid.NewGuid(),
                InvitationReminderCommunicationTemplateId = Guid.NewGuid(),
                TaskReminderCommunicationTemplateId = Guid.NewGuid(),
                RetractedInvitationCommunicationTemplateId = Guid.NewGuid()

            };
            var command = new UpdateElectionCommunicationConfigurationsCommand()
            {
                Id = request.Id,
                ConfirmationOfRegistrationCommunicationTemplateId = request.ConfirmationOfRegistrationCommunicationTemplateId,
                ConfirmationOfCancellationCommunicationTemplateId = request.ConfirmationOfCancellationCommunicationTemplateId,
                InvitationCommunicationTemplateId = request.InvitationCommunicationTemplateId,
                InvitationReminderCommunicationTemplateId = request.InvitationReminderCommunicationTemplateId,
                TaskReminderCommunicationTemplateId = request.TaskReminderCommunicationTemplateId,
                RetractedInvitationCommunicationTemplateId = request.RetractedInvitationCommunicationTemplateId,
                ElectionTaskTypeCommunicationTemplates = request.ElectionTaskTypeCommunicationTemplates
            };
            var handler = new UpdateElectionCommunicationConfigurationsCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateElectionCommunicationConfigurationsAsync(command, default);
        }

        [TestMethod]
        public void UpdateElectionCommunicationConfigurationsCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new UpdateElectionCommunicationConfigurationsCommand()
            {
                ConfirmationOfRegistrationCommunicationTemplateId = Guid.NewGuid(),
                ConfirmationOfCancellationCommunicationTemplateId = Guid.NewGuid(),
                InvitationCommunicationTemplateId = Guid.NewGuid(),
                InvitationReminderCommunicationTemplateId = Guid.NewGuid(),
                TaskReminderCommunicationTemplateId = Guid.NewGuid(),
                RetractedInvitationCommunicationTemplateId = Guid.NewGuid()
            };
            var validator = new UpdateElectionCommunicationConfigurationsCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
