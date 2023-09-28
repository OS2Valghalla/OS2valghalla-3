using Valghalla.Database.Entities.Tables;
using Microsoft.EntityFrameworkCore;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class ConfigurationConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationEntity>(entity =>
            {
                entity.ToTable("Configuration");

                entity.HasKey(e => e.Key);
                entity.Property(e => e.Value).IsRequired();
            });
        }
    }
}
