using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Team.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Team.Queries
{
    [TestClass]
    public class GetTeamQueryHandlerTests
    {
        private readonly ITeamQueryRepository _mockQueryRepository;

        public GetTeamQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<ITeamQueryRepository>();
        }

        [TestMethod]
        public async Task GetTeamQueryHandler_Should_CallOnRepository()
        {
            var query = new GetTeamQuery(Guid.NewGuid());
            var handler = new GetTeamQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetTeamAsync(query, default);
        }

        [TestMethod]
        public void GetTeamQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new GetTeamQuery(Guid.Empty);
            var validator = new GetTeamQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.TeamId);
        }
    }
}
