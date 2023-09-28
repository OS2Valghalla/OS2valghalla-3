using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.ElectionType;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Commands;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.ElectionType.Commands
{
    [TestClass]
    public class CreateElectionTypeCommandHandlerTests
    {
        private readonly IElectionTypeCommandRepository _mockCommandRepository;
        private readonly IElectionTypeQueryRepository _mockQueryRepository;

        public CreateElectionTypeCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionTypeCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionTypeQueryRepository>();
        }

        [TestMethod]
        public async Task CreateElectionTypeCommandHandler_Should_CallCreateElectionTypeAsyncOnRepository()
        {
            var request = new CreateElectionTypeRequest()
            {
                Title = "Test Election Type",
            };
            var command = new CreateElectionTypeCommand(request.Title, request.ValidationRuleIds);
            var handler = new CreateElectionTypeCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).CreateElectionTypeAsync(command, default);
        }

        [TestMethod]
        public void CreateElectionTypeCommandHandler_Should_ReturnValidationErrorOnEmptyTitle()
        {
            var command = new CreateElectionTypeCommand(string.Empty, new List<Guid>());
            var validator = new CreateElectionTypeCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Title);
        }
    }
}
