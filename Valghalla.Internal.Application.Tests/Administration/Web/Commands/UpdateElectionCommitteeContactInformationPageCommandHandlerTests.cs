using NSubstitute;
using Valghalla.Application;
using Valghalla.Internal.API.Requests.Administration.Web;
using Valghalla.Internal.Application.Modules.Administration.Web.Commands;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Web.Commands
{
    [TestClass]
    public class UpdateElectionCommitteeContactInformationPageCommandHandlerTests
    {
        private readonly IElectionCommitteeContactInformationCommandRepository _mockCommandRepository;

        public UpdateElectionCommitteeContactInformationPageCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionCommitteeContactInformationCommandRepository>();
        }

        [TestMethod]
        public async Task UpdateFAQPageCommandHandlerTests_Should_CallUpdateWebPageAsyncOnRepository()
        {
            var request = new UpdateElectionCommitteeContactInformationRequest()
            {
                MunicipalityName = "Testing Municipality",
                ElectionResponsibleApartment = "Testing Responsible Apartment",
                Address = "Testing Address",
                PostalCode = "1234",
                City = "Testing City",

            };
            var command = new UpdateElectionCommitteeContactInformationPageCommand()
            {
                MunicipalityName = request.MunicipalityName,
                ElectionResponsibleApartment = request.ElectionResponsibleApartment,
                Address = request.Address,
                PostalCode = request.PostalCode,
                City = request.City,
            };
            var handler = new UpdateElectionCommitteeContactInformationPageCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateWebPageAsync(Constants.WebPages.WebPageName_ElectionCommitteeContactInformation, command, default);
        }
    }
}
