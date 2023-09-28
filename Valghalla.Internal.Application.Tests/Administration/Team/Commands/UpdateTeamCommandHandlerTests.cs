using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.AuditLog;
using Valghalla.Internal.API.Requests.Administration.Team;
using Valghalla.Internal.Application.Modules.Administration.Team.Commands;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Team.Commands
{
    [TestClass]
    public class UpdateTeamCommandHandlerTests
    {
        private readonly ITeamCommandRepository _mockCommandRepository;
        private readonly ITeamQueryRepository _mockQueryRepository;
        private readonly IParticipantSharedQueryRepository _mockParticipantSharedQueryRepository;
        private readonly IAuditLogService _mockAuditLogService;

        public UpdateTeamCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ITeamCommandRepository>();
            _mockQueryRepository = Substitute.For<ITeamQueryRepository>();
            _mockParticipantSharedQueryRepository = Substitute.For<IParticipantSharedQueryRepository>();
            _mockAuditLogService = Substitute.For<IAuditLogService>();
        }

        [TestMethod]
        public async Task UpdateTeamCommandHandlerTests_Should_CallUpdateTeamAsyncOnRepository()
        {
            var request = new UpdateTeamRequest()
            {
                Id = Guid.NewGuid(),
                Name = "Testing Team",
                ShortName = "ST",
                Description = "Testing Description"
            };
            var command = new UpdateTeamCommand()
            {
                Id = request.Id,
                Name = request.Name,
                ShortName = request.ShortName,
                Description = request.Description,
            };

            _mockCommandRepository
                .UpdateTeamAsync(command, default)
                .Returns(Task.FromResult((Enumerable.Empty<Guid>(), Enumerable.Empty<Guid>())));

            var handler = new UpdateTeamCommandHandler(_mockCommandRepository, _mockParticipantSharedQueryRepository, _mockAuditLogService, new MockQueueService());

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateTeamAsync(command, default);
        }

        [TestMethod]
        public void UpdateTeamCommandHandlerTests_Should_ReturnValidationErrorOnEmptyId()
        {
            var request = new UpdateTeamRequest()
            {
                Name = "Testing Team",
                ShortName = "ST",
                Description = "Testing Description"
            };
            var command = new UpdateTeamCommand()
            {
                Name = request.Name,
                ShortName = request.ShortName,
                Description = request.Description,
            };
            var validator = new UpdateTeamCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void UpdateTeamCommandHandlerTests_Should_ReturnValidationErrorOnEmptyName()
        {
            var request = new UpdateTeamRequest()
            {
                Id = Guid.NewGuid(),
                ShortName = "ST",
                Description = "Testing Description"
            };
            var command = new UpdateTeamCommand()
            {
                Id = request.Id,
                ShortName = request.ShortName,
                Description = request.Description,
            };
            var validator = new UpdateTeamCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Name);
        }

        [TestMethod]
        public void UpdateTeamCommandHandlerTests_Should_ReturnValidationErrorOnEmptyShortName()
        {
            var request = new UpdateTeamRequest()
            {
                Id = Guid.NewGuid(),
                Name = "Testing Team",
                Description = "Testing Description"
            };
            var command = new UpdateTeamCommand()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
            };
            var validator = new UpdateTeamCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ShortName);
        }

        [TestMethod]
        public void UpdateTeamCommandHandlerTests_Should_ReturnValidationErrorOnDuplicatedTeam()
        {
            var request = new UpdateTeamRequest()
            {
                Id = Guid.NewGuid(),
                Name = "Testing Team",
                ShortName = "ST",
                Description = "Testing Description"
            };
            var command = new UpdateTeamCommand()
            {
                Id = request.Id,
                Name = request.Name,
                ShortName = request.ShortName,
                Description = request.Description,
            };
            var validator = new UpdateTeamCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfTeamExistsAsync(command, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.teams.error.team_exist");
        }
    }
}
