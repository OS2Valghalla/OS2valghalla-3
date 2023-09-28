using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.AuditLog;
using Valghalla.Internal.API.Requests.Administration.WorkLocation;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.WorkLocation.Commands
{
    [TestClass]
    public class CreateWorkLocationCommandHandlerTests
    {
        private readonly IWorkLocationCommandRepository _mockCommandRepository;
        private readonly IWorkLocationQueryRepository _mockQueryRepository;
        private readonly IParticipantSharedQueryRepository _mockParticipantSharedQueryRepository;
        private readonly IAuditLogService _mockAuditLogService;

        public CreateWorkLocationCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IWorkLocationCommandRepository>();
            _mockQueryRepository = Substitute.For<IWorkLocationQueryRepository>();
            _mockParticipantSharedQueryRepository = Substitute.For<IParticipantSharedQueryRepository>();
            _mockAuditLogService = Substitute.For<IAuditLogService>();
        }

        [TestMethod]
        public async Task CreateWorkLocationCommandHandlerTests_Should_CallCreateWorkLocationAsyncOnRepository()
        {
            var request = new CreateWorkLocationRequest()
            {
                Title = "Testing Work Location",
                AreaId = Guid.NewGuid(),
                Address = "Testing Address",
                PostalCode = "1234",
                City = "Testing City",
                TaskTypeIds = new List<Guid> { Guid.NewGuid() },
                TeamIds = new List<Guid> { Guid.NewGuid() },
                ResponsibleIds = new List<Guid> { Guid.NewGuid() },
            };
            var command = new CreateWorkLocationCommand()
            {                
                Title = request.Title,
                AreaId = request.AreaId,
                Address = request.Address,
                PostalCode = request.PostalCode,
                City = request.City,
                TaskTypeIds = request.TaskTypeIds,
                TeamIds = request.TeamIds,
                ResponsibleIds = request.ResponsibleIds
            };
            var handler = new CreateWorkLocationCommandHandler(_mockCommandRepository, _mockParticipantSharedQueryRepository, _mockAuditLogService, new MockQueueService());

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).CreateWorkLocationAsync(command, default);
        }

        [TestMethod]
        public void CreateWorkLocationCommandHandlerTests_Should_ReturnValidationErrorOnEmptyTitle()
        {
            var request = new CreateWorkLocationRequest()
            {
                AreaId = Guid.NewGuid(),
                Address = "Testing Address",
                PostalCode = "1234",
                City = "Testing City",
                TaskTypeIds = new List<Guid> { Guid.NewGuid() },
                TeamIds = new List<Guid> { Guid.NewGuid() },
                ResponsibleIds = new List<Guid> { Guid.NewGuid() },
            };
            var command = new CreateWorkLocationCommand()
            {
                AreaId = request.AreaId,
                Address = request.Address,
                PostalCode = request.PostalCode,
                City = request.City,
                TaskTypeIds = request.TaskTypeIds,
                TeamIds = request.TeamIds,
                ResponsibleIds = request.ResponsibleIds
            };
            var validator = new CreateWorkLocationCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Title);
        }

        [TestMethod]
        public void CreateWorkLocationCommandHandlerTests_Should_ReturnValidationErrorOnEmptyAreaId()
        {
            var request = new CreateWorkLocationRequest()
            {
                Title = "Testing Work Location",
                Address = "Testing Address",
                PostalCode = "1234",
                City = "Testing City",
                TaskTypeIds = new List<Guid> { Guid.NewGuid() },
                TeamIds = new List<Guid> { Guid.NewGuid() },
                ResponsibleIds = new List<Guid> { Guid.NewGuid() },
            };
            var command = new CreateWorkLocationCommand()
            {
                Title = request.Title,
                Address = request.Address,
                PostalCode = request.PostalCode,
                City = request.City,
                TaskTypeIds = request.TaskTypeIds,
                TeamIds = request.TeamIds,
                ResponsibleIds = request.ResponsibleIds
            };
            var validator = new CreateWorkLocationCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.AreaId);
        }

        [TestMethod]
        public void CreateWorkLocationCommandHandlerTests_Should_ReturnValidationErrorOnEmptyAddress()
        {
            var request = new CreateWorkLocationRequest()
            {
                Title = "Testing Work Location",
                AreaId = Guid.NewGuid(),
                PostalCode = "1234",
                City = "Testing City",
                TaskTypeIds = new List<Guid> { Guid.NewGuid() },
                TeamIds = new List<Guid> { Guid.NewGuid() },
                ResponsibleIds = new List<Guid> { Guid.NewGuid() },
            };
            var command = new CreateWorkLocationCommand()
            {
                Title = request.Title,
                AreaId = request.AreaId,
                PostalCode = request.PostalCode,
                City = request.City,
                TaskTypeIds = request.TaskTypeIds,
                TeamIds = request.TeamIds,
                ResponsibleIds = request.ResponsibleIds
            };
            var validator = new CreateWorkLocationCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Address);
        }

        [TestMethod]
        public void CreateWorkLocationCommandHandlerTests_Should_ReturnValidationErrorOnEmptyPostalCode()
        {
            var request = new CreateWorkLocationRequest()
            {
                Title = "Testing Work Location",
                AreaId = Guid.NewGuid(),
                Address = "Testing Address",
                City = "Testing City",
                TaskTypeIds = new List<Guid> { Guid.NewGuid() },
                TeamIds = new List<Guid> { Guid.NewGuid() },
                ResponsibleIds = new List<Guid> { Guid.NewGuid() },
            };
            var command = new CreateWorkLocationCommand()
            {
                Title = request.Title,
                AreaId = request.AreaId,
                Address = request.Address,
                City = request.City,
                TaskTypeIds = request.TaskTypeIds,
                TeamIds = request.TeamIds,
                ResponsibleIds = request.ResponsibleIds
            };
            var validator = new CreateWorkLocationCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.PostalCode);
        }

        [TestMethod]
        public void CreateWorkLocationCommandHandlerTests_Should_ReturnValidationErrorOnEmptyCity()
        {
            var request = new CreateWorkLocationRequest()
            {
                Title = "Testing Work Location",
                AreaId = Guid.NewGuid(),
                Address = "Testing Address",
                PostalCode = "1234",
                TaskTypeIds = new List<Guid> { Guid.NewGuid() },
                TeamIds = new List<Guid> { Guid.NewGuid() },
                ResponsibleIds = new List<Guid> { Guid.NewGuid() },
            };
            var command = new CreateWorkLocationCommand()
            {
                Title = request.Title,
                AreaId = request.AreaId,
                Address = request.Address,
                PostalCode = request.PostalCode,
                TaskTypeIds = request.TaskTypeIds,
                TeamIds = request.TeamIds,
                ResponsibleIds = request.ResponsibleIds
            };
            var validator = new CreateWorkLocationCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.City);
        }

        [TestMethod]
        public void CreateWorkLocationCommandHandlerTests_Should_ReturnValidationErrorOnDuplicatedWorkLocation()
        {
            var request = new CreateWorkLocationRequest()
            {
                Title = "Testing Work Location",
                AreaId = Guid.NewGuid(),
                Address = "Testing Address",
                PostalCode = "1234",
                City = "Testing City",
                TaskTypeIds = new List<Guid> { Guid.NewGuid() },
                TeamIds = new List<Guid> { Guid.NewGuid() },
                ResponsibleIds = new List<Guid> { Guid.NewGuid() },
            };
            var command = new CreateWorkLocationCommand()
            {
                Title = request.Title,
                AreaId = request.AreaId,
                Address = request.Address,
                PostalCode = request.PostalCode,
                City = request.City,
                TaskTypeIds = request.TaskTypeIds,
                TeamIds = request.TeamIds,
                ResponsibleIds = request.ResponsibleIds
            };
            var validator = new CreateWorkLocationCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfWorkLocationExistsAsync(command, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.work_location.error.work_location_exist");
        }
    }
}
