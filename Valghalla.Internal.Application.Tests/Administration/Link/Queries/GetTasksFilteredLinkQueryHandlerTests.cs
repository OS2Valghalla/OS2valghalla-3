using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Link.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Link.Queries
{
    [TestClass]
    public class GetTasksFilteredLinkQueryHandlerTests
    {
        private readonly ILinkQueryRepository _mockQueryRepository;
        public GetTasksFilteredLinkQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<ILinkQueryRepository>();
        }

        [TestMethod]
        public async Task GetTasksFilteredLinkQueryHandler_Should_CallGetTaskLinkAsyncOnRepository()
        {
            var query = new GetTasksFilteredLinkQuery("TestHashValue", Guid.NewGuid());
            var handler = new GetTasksFilteredLinkQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetTasksFilteredLinkAsync(query, default);
        }

        [TestMethod]
        public void GetTasksFilteredLinkQueryHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new GetTasksFilteredLinkQuery("TestHashValue", Guid.Empty);
            var validator = new GetTasksFilteredLinkQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }

        [TestMethod]
        public void GetTasksFilteredLinkQueryHandler_Should_ReturnValidationErrorOnEmptyHashValue()
        {
            var command = new GetTasksFilteredLinkQuery(string.Empty, Guid.NewGuid());
            var validator = new GetTasksFilteredLinkQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.HashValue);
        }
    }
}
