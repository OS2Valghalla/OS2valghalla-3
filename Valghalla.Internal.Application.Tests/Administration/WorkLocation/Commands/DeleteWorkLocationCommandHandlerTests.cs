using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.AuditLog;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.WorkLocation.Commands
{
    [TestClass]
    public class DeleteWorkLocationCommandHandlerTests
    {
        private readonly IWorkLocationCommandRepository _mockCommandRepository;
        private readonly IWorkLocationQueryRepository _mockQueryRepository;
        private readonly IParticipantSharedQueryRepository _mockParticipantSharedQueryRepository;
        private readonly IAuditLogService _mockAuditLogService;

        public DeleteWorkLocationCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IWorkLocationCommandRepository>();
            _mockQueryRepository = Substitute.For<IWorkLocationQueryRepository>();
            _mockParticipantSharedQueryRepository = Substitute.For<IParticipantSharedQueryRepository>();
            _mockAuditLogService = Substitute.For<IAuditLogService>();
        }

        [TestMethod]
        public async Task DeleteWorkLocationCommandHandler_Should_CallDeleteWorkLocationAsyncOnRepository()
        {
            Guid id = Guid.NewGuid();
            var command = new DeleteWorkLocationCommand(id);
            var handler = new DeleteWorkLocationCommandHandler(_mockCommandRepository, _mockParticipantSharedQueryRepository, _mockAuditLogService, new MockQueueService());

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DeleteWorkLocationAsync(command, default);
        }

        [TestMethod]
        public void DeleteWorkLocationCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteWorkLocationCommand(Guid.Empty);
            var validator = new DeleteWorkLocationCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void DeleteWorkLocationCommandHandler_Should_ReturnValidationErrorOnUsedInActiveElection()
        {
            Guid id = Guid.NewGuid();

            var command = new DeleteWorkLocationCommand(id);

            _mockQueryRepository
                .CheckIfWorkLocationUsedInActiveElectionAsync(id, default)
                .Returns(Task.FromResult(true));

            var validator = new DeleteWorkLocationCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.work_location.error.work_location_used_in_active_election");
        }
    }
}
