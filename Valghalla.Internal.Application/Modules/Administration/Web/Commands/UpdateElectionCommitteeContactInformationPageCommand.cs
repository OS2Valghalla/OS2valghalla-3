using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Commands
{
    public sealed record UpdateElectionCommitteeContactInformationPageCommand() : ICommand<Response>
    {
        public string MunicipalityName { get; set; } = null!;
        public string ElectionResponsibleApartment { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? TelephoneNumber { get; set; }
        public string? Email { get; set; }
        public string? DigitalPost { get; set; }
        public Guid? LogoFileReferenceId { get; set; }
    }

    internal class UpdateElectionCommitteeContactInformationPageCommandHandler : ICommandHandler<UpdateElectionCommitteeContactInformationPageCommand, Response>
    {
        private readonly IElectionCommitteeContactInformationCommandRepository webPageCommandRepository;
        public UpdateElectionCommitteeContactInformationPageCommandHandler(IElectionCommitteeContactInformationCommandRepository webPageCommandRepository)
        {
            this.webPageCommandRepository = webPageCommandRepository;
        }

        public async Task<Response> Handle(UpdateElectionCommitteeContactInformationPageCommand command, CancellationToken cancellationToken)
        {
            await webPageCommandRepository.UpdateWebPageAsync(Constants.WebPages.WebPageName_ElectionCommitteeContactInformation, command, cancellationToken);
            return Response.Ok();
        }
    }
}
