using Microsoft.EntityFrameworkCore;
using Valghalla.Database;

namespace Valghalla.Tools.Utils
{
    public static class ContextUtils
    {
        public static DataContext CreateDbContext(string connection)
        {
            DbContextOptionsBuilder<DataContext> optionsBuilder = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql(connection);

            return new DataContext(optionsBuilder.Options);
        }
    }
}
