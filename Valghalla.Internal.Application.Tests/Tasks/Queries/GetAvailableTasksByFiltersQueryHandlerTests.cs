using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.Configuration;
using Valghalla.Application.Tenant;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

namespace Valghalla.Internal.Application.Tests.Tasks.Queries
{
    [TestClass]
    public class GetAvailableTasksByFiltersQueryHandlerTests
    {
        private readonly IFilteredTasksQueryRepository _mockQueryRepository;
        private readonly ITenantContextProvider _mockTenantContextProvider;

        public GetAvailableTasksByFiltersQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IFilteredTasksQueryRepository>();
            _mockTenantContextProvider = new MockTennantContextProvider();
        }

        [TestMethod]
        public async Task GetAvailableTasksByFiltersQueryHandler_Should_CallOnRepository()
        {
            var query = new GetAvailableTasksByFiltersQuery(Guid.NewGuid(), new Modules.Tasks.Requests.TasksFilterRequest());
            var handler = new GetAvailableTasksByFiltersQueryHandler(_mockTenantContextProvider, _mockQueryRepository);

            await handler.Handle(query, default);

            string taskDetailsPageUrl = _mockTenantContextProvider.CurrentTenant.ExternalDomain.TrimEnd('/') + "/opgaver/detaljer/";

            await _mockQueryRepository.Received(1).GetAvailableTasksByFiltersAsync(query, taskDetailsPageUrl, default);
        }

        [TestMethod]
        public void GetAvailableTasksByFiltersQueryHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new GetAvailableTasksByFiltersQuery(Guid.Empty, new Modules.Tasks.Requests.TasksFilterRequest());
            var validator = new GetAvailableTasksByFiltersQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }
    }
}
