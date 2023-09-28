using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.Web;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Web
{
    public class WebPageQueryRepository : IWebPageQueryRepository
    {
        private readonly IQueryable<WebPageEntity> webPages;
        private readonly IMapper mapper;

        public WebPageQueryRepository(DataContext dataContext, IMapper mapper)
        {
            webPages = dataContext.Set<WebPageEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<WebPage?> GetWebPageAsync(string pageName, CancellationToken cancellationToken)
        {
            var webPage = await webPages
               .FirstOrDefaultAsync(i => i.PageName == pageName, cancellationToken);

            if (webPage == null) { return null; }

            return mapper.Map<WebPage>(webPage);
        }
    }
}
