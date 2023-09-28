using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.Tenant;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

namespace Valghalla.Internal.Application.Tests.Tasks.Queries
{
    [TestClass]
    public class GetTeamTasksQueryHandlerTests
    {
        private readonly IElectionWorkLocationTasksQueryRepository _mockQueryRepository;
        private readonly ITenantContextProvider _mockTenantContextProvider;

        public GetTeamTasksQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IElectionWorkLocationTasksQueryRepository>();
            _mockTenantContextProvider = new MockTennantContextProvider();
        }

        [TestMethod]
        public async Task GetTeamTasksQueryHandler_Should_CallOnRepository()
        {
            var query = new GetTeamTasksQuery(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), false);
            var handler = new GetTeamTasksQueryHandler(_mockTenantContextProvider, _mockQueryRepository);

            await handler.Handle(query, default);

            string taskDetailsPageUrl = _mockTenantContextProvider.CurrentTenant.ExternalDomain.TrimEnd('/') + "/opgaver/detaljer/";

            await _mockQueryRepository.Received(1).GetTeamTasksAsync(query, taskDetailsPageUrl, default);
        }

        [TestMethod]
        public void GetTeamTasksQueryHandler_Should_ReturnValidationErrorOnEmptyTeamId()
        {
            var command = new GetTeamTasksQuery(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), false);
            var validator = new GetTeamTasksQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.TeamId);
        }

        [TestMethod]
        public void GetTeamTasksQueryHandler_Should_ReturnValidationErrorOnEmptyWorkLocationId()
        {
            var command = new GetTeamTasksQuery(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), false);
            var validator = new GetTeamTasksQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.WorkLocationId);
        }

        [TestMethod]
        public void GetTeamTasksQueryHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new GetTeamTasksQuery(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, false);
            var validator = new GetTeamTasksQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }
    }
}
