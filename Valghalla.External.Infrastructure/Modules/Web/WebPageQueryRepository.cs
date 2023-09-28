using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.Web;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Web.Interfaces;

namespace Valghalla.External.Infrastructure.Modules.Web
{
    internal class WebPageQueryRepository : IWebPageQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<WebPageEntity> webPages;

        public WebPageQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            webPages = dataContext.Set<WebPageEntity>().AsNoTracking();
        }

        public async Task<WebPage?> GetWebPageAsync(string pageName, CancellationToken cancellationToken)
        {
            var webPage = await webPages
               .Where(i => i.PageName == pageName)
               .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<WebPage>(webPage);
        }
    }
}
