using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Web.Commands;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Web
{
    public class ElectionCommitteeContactInformationCommandRepository : IElectionCommitteeContactInformationCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<ElectionCommitteeContactInformationEntity> webPages;

        public ElectionCommitteeContactInformationCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            webPages = dataContext.Set<ElectionCommitteeContactInformationEntity>();
        }

        public async Task<bool> UpdateWebPageAsync(string pageName, UpdateElectionCommitteeContactInformationPageCommand command, CancellationToken cancellationToken)
        {
            var itemToUpdate = webPages.FirstOrDefault(x => x.PageName == pageName);

            if (itemToUpdate == null)
            {
                var itemToAdd = new ElectionCommitteeContactInformationEntity
                {
                    PageName = pageName,
                    MunicipalityName = command.MunicipalityName,
                    ElectionResponsibleApartment = command.ElectionResponsibleApartment,
                    Address = command.Address,
                    PostalCode = command.PostalCode,
                    City = command.City,
                    TelephoneNumber = command.TelephoneNumber,
                    DigitalPost = command.DigitalPost,
                    Email = command.Email,
                    LogoFileReferenceId = command.LogoFileReferenceId
                };

                await webPages.AddAsync(itemToAdd, cancellationToken);
            }
            else
            {
                itemToUpdate.MunicipalityName = command.MunicipalityName;
                itemToUpdate.ElectionResponsibleApartment = command.ElectionResponsibleApartment;
                itemToUpdate.Address = command.Address;
                itemToUpdate.PostalCode = command.PostalCode;
                itemToUpdate.City = command.City;
                itemToUpdate.TelephoneNumber = command.TelephoneNumber;
                itemToUpdate.DigitalPost = command.DigitalPost;
                itemToUpdate.Email = command.Email;
                itemToUpdate.LogoFileReferenceId = command.LogoFileReferenceId;
                webPages.Update(itemToUpdate);
            }

            var updated = await dataContext.SaveChangesAsync(cancellationToken);

            return updated > 0;
        }
    }
}
