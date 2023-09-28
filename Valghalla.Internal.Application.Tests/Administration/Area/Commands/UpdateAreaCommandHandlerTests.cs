using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.Area;
using Valghalla.Internal.Application.Modules.Administration.Area.Commands;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Area.Commands
{
    [TestClass]
    public class UpdateAreaCommandHandlerTests
    {
        private readonly IAreaCommandRepository _mockCommandRepository;
        private readonly IAreaQueryRepository _mockQueryRepository;

        public UpdateAreaCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IAreaCommandRepository>();
            _mockQueryRepository = Substitute.For<IAreaQueryRepository>();
        }

        [TestMethod]
        public async Task UpdateAreaCommandHandler_Should_CallUpdateAreaAsyncOnRepository()
        {
            var request = new UpdateAreaRequest()
            {
                Id = Guid.NewGuid(),
                Name = "Testing Area",
                Description = "Area description"
            };
            var command = new UpdateAreaCommand(request.Id, request.Name, request.Description);
            var handler = new UpdateAreaCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateAreaAsync(command, default);
        }

        [TestMethod]
        public void UpdateAreaCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var request = new UpdateAreaRequest()
            {
                Id = Guid.Empty,
                Name = "Testing Area"
            };
            var command = new UpdateAreaCommand(request.Id, request.Name, request.Description);
            var validator = new UpdateAreaCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void UpdateAreaCommandHandler_Should_ReturnValidationErrorOnEmptyName()
        {
            var request = new UpdateAreaRequest()
            {
                Id = Guid.NewGuid(),
                Name = ""
            };
            var command = new UpdateAreaCommand(request.Id, request.Name, request.Description);
            var validator = new UpdateAreaCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Name);
        }

        [TestMethod]
        public void UpdateAreaCommandHandler_Should_ReturnValidationErrorOnDuplicatedArea()
        {
            var name = "Testing Area";

            var request = new UpdateAreaRequest()
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            var command = new UpdateAreaCommand(request.Id, request.Name, request.Description);
            var validator = new UpdateAreaCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfAreaExistsAsync(command, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.area.error.area_exist");
        }
    }
}
