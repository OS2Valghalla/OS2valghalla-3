using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.Web;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Web
{
    public class ElectionCommitteeContactInformationQueryRepository : IElectionCommitteeContactInformationQueryRepository
    {
        private readonly IQueryable<ElectionCommitteeContactInformationEntity> webPages;
        private readonly IMapper mapper;

        public ElectionCommitteeContactInformationQueryRepository(DataContext dataContext, IMapper mapper)
        {
            webPages = dataContext.Set<ElectionCommitteeContactInformationEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<ElectionCommitteeContactInformationPage?> GetWebPageAsync(string pageName, CancellationToken cancellationToken)
        {
            var webPage = await webPages.Include(i => i.LogoFileReference)
               .FirstOrDefaultAsync(i => i.PageName == pageName, cancellationToken);

            if (webPage == null) { return null; }

            return mapper.Map<ElectionCommitteeContactInformationPage>(webPage);
        }
    }
}
