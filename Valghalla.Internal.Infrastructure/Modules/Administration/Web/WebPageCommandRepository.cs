using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Web
{
    public class WebPageCommandRepository : IWebPageCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<WebPageEntity> webPages;

        public WebPageCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            webPages = dataContext.Set<WebPageEntity>();
        }

        public async Task<bool> UpdateWebPageAsync(string pageName, string pageInfo, CancellationToken cancellationToken)
        {
            var itemToUpdate = webPages.FirstOrDefault(x => x.PageName == pageName);

            if (itemToUpdate == null)
            {
                var itemToAdd = new WebPageEntity
                {
                    PageName = pageName,
                    PageInfo = pageInfo,
                };

                await webPages.AddAsync(itemToAdd, cancellationToken);
            }
            else
            {
                itemToUpdate.PageInfo = pageInfo;
                webPages.Update(itemToUpdate);
            }

            var updated = await dataContext.SaveChangesAsync(cancellationToken);

            return updated > 0;
        }
    }
}
