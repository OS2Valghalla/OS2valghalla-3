using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

namespace Valghalla.Internal.Application.Tests.Tasks.Queries
{
    [TestClass]
    public class GetElectionWorkLocationTasksSummaryQueryHandlerTests
    {
        private readonly IElectionWorkLocationTasksQueryRepository _mockQueryRepository;

        public GetElectionWorkLocationTasksSummaryQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IElectionWorkLocationTasksQueryRepository>();
        }

        [TestMethod]
        public async Task GetElectionWorkLocationTasksSummaryQueryHandler_Should_CallOnRepository()
        {
            var query = new GetElectionWorkLocationTasksSummaryQuery(Guid.NewGuid(), Guid.NewGuid());
            var handler = new GetElectionWorkLocationTasksSummaryQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetElectionWorkLocationTasksSummaryAsync(query, default);
        }

        [TestMethod]
        public void GetElectionWorkLocationTasksSummaryQueryHandler_Should_ReturnValidationErrorOnEmptyWorkLocationId()
        {
            var command = new GetElectionWorkLocationTasksSummaryQuery(Guid.Empty, Guid.NewGuid());
            var validator = new GetElectionWorkLocationTasksSummaryQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.WorkLocationId);
        }

        [TestMethod]
        public void GetElectionWorkLocationTasksSummaryQueryHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new GetElectionWorkLocationTasksSummaryQuery(Guid.NewGuid(), Guid.Empty);
            var validator = new GetElectionWorkLocationTasksSummaryQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }
    }
}
