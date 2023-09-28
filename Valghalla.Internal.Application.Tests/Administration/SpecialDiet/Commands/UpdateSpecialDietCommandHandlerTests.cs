using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.SpecialDiet;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.SpecialDiet.Commands
{
    [TestClass]
    public class UpdateSpecialDietCommandHandlerTests
    {
        private readonly ISpecialDietCommandRepository _mockCommandRepository;
        private readonly ISpecialDietQueryRepository _mockQueryRepository;

        public UpdateSpecialDietCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ISpecialDietCommandRepository>();
            _mockQueryRepository = Substitute.For<ISpecialDietQueryRepository>();
        }

        [TestMethod]
        public async Task UpdateSpecialDietCommandHandler_Should_CallUpdateSpecialDietAsyncOnRepository()
        {
            var request = new UpdateSpecialDietRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Testing Special Diet"
            };
            var command = new UpdateSpecialDietCommand(request.Id, request.Title);
            var handler = new UpdateSpecialDietCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateSpecialDietAsync(command, default);
        }

        [TestMethod]
        public void UpdateSpecialDietCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new UpdateSpecialDietCommand(Guid.Empty, "Testing Special Diet");
            var validator = new UpdateSpecialDietCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void UpdateSpecialDietCommandHandler_Should_ReturnValidationErrorOnEmptyTitle()
        {
            var command = new UpdateSpecialDietCommand(Guid.NewGuid(), string.Empty);
            var validator = new UpdateSpecialDietCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Title);
        }

        [TestMethod]
        public void UpdateSpecialDietCommandHandler_Should_ReturnValidationErrorOnDuplicatedSpecialDiet()
        {
            var command = new UpdateSpecialDietCommand(Guid.NewGuid(), "Testing Special Diet");
            var validator = new UpdateSpecialDietCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfSpecialDietExistsAsync(command, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.specialdiet.error.specialdiet_exist");
        }
    }
}
