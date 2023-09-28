using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.AuditLog;
using Valghalla.Internal.Application.Modules.Administration.Team.Commands;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Team.Commands
{
    [TestClass]
    public class DeleteTeamCommandHandlerTests
    {
        private readonly ITeamCommandRepository _mockCommandRepository;
        private readonly ITeamQueryRepository _mockQueryRepository;
        private readonly IParticipantSharedQueryRepository _mockParticipantSharedQueryRepository;
        private readonly IAuditLogService _mockAuditLogService;

        public DeleteTeamCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ITeamCommandRepository>();
            _mockQueryRepository = Substitute.For<ITeamQueryRepository>();
            _mockParticipantSharedQueryRepository = Substitute.For<IParticipantSharedQueryRepository>();
            _mockAuditLogService = Substitute.For<IAuditLogService>();
        }

        [TestMethod]
        public async Task DeleteTeamCommandHandler_Should_CallDeleteTeamAsyncOnRepository()
        {
            Guid id = Guid.NewGuid();
            var command = new DeleteTeamCommand(id);
            var handler = new DeleteTeamCommandHandler(_mockCommandRepository, _mockParticipantSharedQueryRepository, _mockAuditLogService, new MockQueueService());

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DeleteTeamAsync(command, default);
        }

        [TestMethod]
        public void DeleteTeamCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteTeamCommand(Guid.Empty);
            var validator = new DeleteTeamCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void DeleteTeamCommandHandler_Should_ReturnValidationErrorOnUsedInActiveElection()
        {
            Guid id = Guid.NewGuid();

            var command = new DeleteTeamCommand(id);

            _mockQueryRepository
                .CheckIfTeamUsedInActiveElectionAsync(id, default)
                .Returns(Task.FromResult(true));

            var validator = new DeleteTeamCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.teams.error.team_used_in_active_election");
        }
    }
}
