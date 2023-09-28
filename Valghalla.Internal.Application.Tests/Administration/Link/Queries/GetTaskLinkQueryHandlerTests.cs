using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Link.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Link.Queries
{
    [TestClass]
    public class GetTaskLinkQueryHandlerTests
    {
        private readonly ILinkQueryRepository _mockQueryRepository;

        public GetTaskLinkQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<ILinkQueryRepository>();
        }

        [TestMethod]
        public async Task GetTaskLinkQueryHandler_Should_CallGetTaskLinkAsyncOnRepository()
        {
            var command = new GetTaskLinkQuery("TestHashValue", Guid.NewGuid());
            var handler = new GetTaskLinkQueryHandler(_mockQueryRepository);

            await handler.Handle(command, default);

            await _mockQueryRepository.Received(1).GetTaskLinkAsync(command, default);
        }

        [TestMethod]
        public void GetTaskLinkQueryHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new GetTaskLinkQuery("TestHashValue", Guid.Empty);
            var validator = new GetTaskLinkQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }

        [TestMethod]
        public void GetTaskLinkQueryHandler_Should_ReturnValidationErrorOnEmptyHashValue()
        {
            var command = new GetTaskLinkQuery(string.Empty, Guid.NewGuid());
            var validator = new GetTaskLinkQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.HashValue);
        }
    }
}
