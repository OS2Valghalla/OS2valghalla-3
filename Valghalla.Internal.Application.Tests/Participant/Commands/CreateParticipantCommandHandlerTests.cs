using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.Communication;
using Valghalla.Application.CPR;
using Valghalla.Application.TaskValidation;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Tests.Participant.Commands
{
    [TestClass]
    public class CreateParticipantCommandHandlerTests
    {
        private readonly ICPRService _mockCprService;
        private readonly ITaskValidationService _mockTaskValidationService;
        private readonly IParticipantCommandRepository _mockParticipantCommandRepository;
        private readonly IParticipantQueryRepository _mockParticipantQueryRepository;
        private readonly IElectionWorkLocationTasksCommandRepository _mockElectionWorkLocationTasksCommandRepository;
        private readonly ICommunicationService _mockCommunicationService;

        public CreateParticipantCommandHandlerTests()
        {
            _mockCprService = Substitute.For<ICPRService>();
            _mockTaskValidationService = Substitute.For<ITaskValidationService>();
            _mockParticipantCommandRepository = Substitute.For<IParticipantCommandRepository>();
            _mockParticipantQueryRepository = Substitute.For<IParticipantQueryRepository>();
            _mockElectionWorkLocationTasksCommandRepository = Substitute.For<IElectionWorkLocationTasksCommandRepository>();
            _mockCommunicationService = Substitute.For<ICommunicationService>();
        }

        [TestMethod]
        public async Task CreateParticipantCommandHandler_Should_CallExecuteAsyncOnCprServiceAndCreateParticipantAsyncOnRepository()
        {
            var command = new CreateParticipantCommand()
            {
                Cpr = "1208659999",
                MobileNumber = "+457778888",
                Email = "test@net.xyz",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = new[] { Guid.NewGuid() }
            };

            var handler = new CreateParticipantCommandHandler(
                _mockCprService,
                _mockCommunicationService,
                _mockParticipantCommandRepository, _mockElectionWorkLocationTasksCommandRepository);

            _mockCprService
                .ExecuteAsync(command.Cpr)
                .Returns(Task.FromResult(new CprPersonInfo()
                {
                    Address = new(),
                    Municipality = new(),
                    Country = new(),
                }));

            await handler.Handle(command, default);

            await _mockCprService.Received(1).ExecuteAsync(command.Cpr);

            await _mockParticipantCommandRepository.Received(1).CreateParticipantAsync(command, Arg.Any<ParticipantPersonalRecord>(), default);
        }

        [TestMethod]
        public void CreateParticipantCommandHandler_Should_ReturnValidationErrorOnEmptyCpr()
        {
            var command = new CreateParticipantCommand()
            {
                Cpr = string.Empty,
                MobileNumber = "+457778888",
                Email = "test@net.xyz",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = new[] { Guid.NewGuid() }
            };

            var validator = new CreateParticipantCommandValidator(_mockParticipantQueryRepository, _mockTaskValidationService);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Cpr);
        }

        [TestMethod]
        public void CreateParticipantCommandHandler_Should_ReturnValidationErrorOnInvalidEmail()
        {
            var command = new CreateParticipantCommand()
            {
                Cpr = "1208659999",
                MobileNumber = "+457778888",
                Email = "abc123",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = new[] { Guid.NewGuid() }
            };

            var validator = new CreateParticipantCommandValidator(_mockParticipantQueryRepository, _mockTaskValidationService);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Email);
        }

        [TestMethod]
        public void CreateParticipantCommandHandler_Should_ReturnValidationErrorOnEmptyTeamIds()
        {
            var command = new CreateParticipantCommand()
            {
                Cpr = "1208659999",
                MobileNumber = "+457778888",
                Email = "test@net.xyz",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = Array.Empty<Guid>()
            };

            var validator = new CreateParticipantCommandValidator(_mockParticipantQueryRepository, _mockTaskValidationService);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.TeamIds);
        }

        [TestMethod]
        public void CreateParticipantCommandHandler_Should_ReturnValidationErrorOnInvalidCpr()
        {
            var command = new CreateParticipantCommand()
            {
                Cpr = "34234234234323",
                MobileNumber = "+457778888",
                Email = "test@net.xyz",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = new[] { Guid.NewGuid() }
            };

            var validator = new CreateParticipantCommandValidator(_mockParticipantQueryRepository, _mockTaskValidationService);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveValidationErrorFor(query => query.Cpr)
                .WithErrorMessage("participant.error.cpr_invalid");
        }

        [TestMethod]
        public void CreateParticipantCommandHandler_Should_ReturnValidationErrorOnDuplicatedParticipant()
        {
            var command = new CreateParticipantCommand()
            {
                Cpr = "1208659999",
                MobileNumber = "+457778888",
                Email = "test@net.xyz",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = new[] { Guid.NewGuid() }
            };

            _mockParticipantQueryRepository
                .CheckIfParticipantExistsAsync(command.Cpr, default)
                .Returns(Task.FromResult(true));

            var validator = new CreateParticipantCommandValidator(_mockParticipantQueryRepository, _mockTaskValidationService);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("participant.error.participant_exists");
        }
    }
}
