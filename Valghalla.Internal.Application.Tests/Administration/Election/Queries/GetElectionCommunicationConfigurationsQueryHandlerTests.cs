using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Election.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Election.Queries
{
    [TestClass]
    public class GetElectionCommunicationConfigurationsQueryHandlerTests
    {
        private readonly IElectionQueryRepository _mockQueryRepository;

        public GetElectionCommunicationConfigurationsQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IElectionQueryRepository>();
        }

        [TestMethod]
        public async Task GetElectionCommunicationConfigurationsQueryHandler_Should_CallOnRepository()
        {
            var query = new GetElectionCommunicationConfigurationsQuery(Guid.NewGuid());
            var handler = new GetElectionCommunicationConfigurationsQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetElectionCommunicationConfigurationsAsync(query, default);
        }

        [TestMethod]
        public void GetElectionCommunicationConfigurationsQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new GetElectionCommunicationConfigurationsQuery(Guid.Empty);
            var validator = new GetElectionCommunicationConfigurationsQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
