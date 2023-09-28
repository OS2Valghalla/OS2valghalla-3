using Valghalla.Database.Entities.Tables;
using Microsoft.EntityFrameworkCore;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class PrintTemplateMappingConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrintTemplateMappingEntity>(entity =>
            {
                entity.HasKey(e => new { e.Entity_EntityName, e.Entity_EntityPropertyName });
            });
        }
    }
}
