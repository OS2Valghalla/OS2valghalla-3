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
    public class CreateTeamCommandHandlerTests
    {
        private readonly ITeamCommandRepository _mockCommandRepository;
        private readonly ITeamQueryRepository _mockQueryRepository;
        private readonly IParticipantSharedQueryRepository _mockParticipantSharedQueryRepository;
        private readonly IAuditLogService _mockAuditLogService;

        public CreateTeamCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ITeamCommandRepository>();
            _mockQueryRepository = Substitute.For<ITeamQueryRepository>();
            _mockParticipantSharedQueryRepository = Substitute.For<IParticipantSharedQueryRepository>();
            _mockAuditLogService = Substitute.For<IAuditLogService>();
        }

        [TestMethod]
        public async Task CreateTeamCommandHandlerTests_Should_CallCreateTeamAsyncOnRepository()
        {
            var request = new CreateTeamRequest()
            {
                Name = "Testing Team",
                ShortName = "ST",
                Description = "Testing Description"
            };
            var command = new CreateTeamCommand()
            {
                Name = request.Name,
                ShortName = request.ShortName,
                Description = request.Description,
            };
            var handler = new CreateTeamCommandHandler(_mockCommandRepository, _mockParticipantSharedQueryRepository, _mockAuditLogService, new MockQueueService());

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).CreateTeamAsync(command, default);
        }

        [TestMethod]
        public void CreateTeamCommandHandlerTests_Should_ReturnValidationErrorOnEmptyName()
        {
            var request = new CreateTeamRequest()
            {
                ShortName = "ST",
                Description = "Testing Description"
            };
            var command = new CreateTeamCommand()
            {
                ShortName = request.ShortName,
                Description = request.Description,
            };
            var validator = new CreateTeamCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Name);
        }

        [TestMethod]
        public void CreateTeamCommandHandlerTests_Should_ReturnValidationErrorOnEmptyShortName()
        {
            var request = new CreateTeamRequest()
            {
                Name = "Testing Team",
                Description = "Testing Description"
            };
            var command = new CreateTeamCommand()
            {
                Name = request.Name,
                Description = request.Description,
            };
            var validator = new CreateTeamCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ShortName);
        }

        [TestMethod]
        public void CreateTeamCommandHandlerTests_Should_ReturnValidationErrorOnDuplicatedTeam()
        {
            var request = new CreateTeamRequest()
            {
                Name = "Testing Team",
                ShortName = "ST",
                Description = "Testing Description"
            };
            var command = new CreateTeamCommand()
            {
                Name = request.Name,
                ShortName = request.ShortName,
                Description = request.Description,
            };
            var validator = new CreateTeamCommandValidator(_mockQueryRepository);

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
