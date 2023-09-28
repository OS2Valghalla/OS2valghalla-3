using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.ElectionType;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Commands;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.ElectionType.Commands
{
    [TestClass]
    public class UpdateElectionTypeCommandHandlerTests
    {
        private readonly IElectionTypeCommandRepository _mockCommandRepository;
        private readonly IElectionTypeQueryRepository _mockQueryRepository;

        public UpdateElectionTypeCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionTypeCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionTypeQueryRepository>();
        }

        [TestMethod]
        public async Task UpdateElectionTypeCommandHandler_Should_CallUpdateElectionTypeAsyncOnRepository()
        {
            var request = new UpdateElectionTypeRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Test Election Type",
            };
            var command = new UpdateElectionTypeCommand(request.Id, request.Title, request.ValidationRuleIds);
            var handler = new UpdateElectionTypeCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateElectionTypeAsync(command, default);
        }

        [TestMethod]
        public void UpdateElectionTypeCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var request = new UpdateElectionTypeRequest()
            {
                Title = "Test Election Type",
            };
            var command = new UpdateElectionTypeCommand(request.Id, request.Title, new List<Guid>());
            var validator = new UpdateElectionTypeCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void UpdateElectionTypeCommandHandler_Should_ReturnValidationErrorOnEmptyTitle()
        {
            var request = new UpdateElectionTypeRequest()
            {
                Id = Guid.NewGuid(),
            };
            var command = new UpdateElectionTypeCommand(request.Id, request.Title, new List<Guid>());
            var validator = new UpdateElectionTypeCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Title);
        }
    }
}
