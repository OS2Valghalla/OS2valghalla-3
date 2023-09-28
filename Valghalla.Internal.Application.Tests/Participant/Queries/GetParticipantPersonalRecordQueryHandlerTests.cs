using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.AuditLog;
using Valghalla.Application.CPR;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Participant.Queries;

namespace Valghalla.Internal.Application.Tests.Participant.Queries
{
    [TestClass]
    public class GetParticipantPersonalRecordQueryHandlerTests
    {
        private readonly ICPRService _mockCprService;
        private readonly IParticipantQueryRepository _mockParticipantQueryRepository;
        private readonly IAuditLogService _mockAuditLogService;

        public GetParticipantPersonalRecordQueryHandlerTests()
        {
            _mockCprService = Substitute.For<ICPRService>();
            _mockParticipantQueryRepository = Substitute.For<IParticipantQueryRepository>();
            _mockAuditLogService = Substitute.For<IAuditLogService>();
        }

        [TestMethod]
        public async Task GetParticipantPersonalRecordQueryHandler_Should_CallExecuteAsyncOnCprService()
        {
            var cpr = "1208659999";
            var query = new GetParticipantPersonalRecordQuery(cpr);
            var handler = new GetParticipantPersonalRecordQueryHandler(_mockCprService, _mockAuditLogService);

            _mockCprService
                .ExecuteAsync(cpr)
                .Returns(Task.FromResult(new CprPersonInfo()
                {
                    Address = new(),
                    Municipality = new(),
                    Country = new(),
                }));

            await handler.Handle(query, default);

            await _mockCprService.Received(1).ExecuteAsync(cpr);
        }

        [TestMethod]
        public void GetParticipantPersonalRecordQueryHandler_Should_ReturnValidationErrorOnEmptyCpr()
        {
            var query = new GetParticipantPersonalRecordQuery(string.Empty);
            var validator = new GetParticipantPersonalRecordQueryValidator(_mockParticipantQueryRepository);
            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(query => query.Cpr);
        }

        [TestMethod]
        public void GetParticipantPersonalRecordQueryHandler_Should_ReturnValidationErrorOnInvalidCpr()
        {
            var query = new GetParticipantPersonalRecordQuery("32321323423");
            var validator = new GetParticipantPersonalRecordQueryValidator(_mockParticipantQueryRepository);

            var result = validator.TestValidate(query);

            result
                .ShouldHaveValidationErrorFor(query => query.Cpr)
                .WithErrorMessage("participant.error.cpr_invalid");
        }

        [TestMethod]
        public void GetParticipantPersonalRecordQueryHandler_Should_ReturnValidationErrorOnDuplicatedParticipant()
        {
            var command = new GetParticipantPersonalRecordQuery("1208659999");

            _mockParticipantQueryRepository
                .CheckIfParticipantExistsAsync(command.Cpr, default)
                .Returns(Task.FromResult(true));

            var validator = new GetParticipantPersonalRecordQueryValidator(_mockParticipantQueryRepository);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("participant.error.participant_exists");
        }
    }
}
