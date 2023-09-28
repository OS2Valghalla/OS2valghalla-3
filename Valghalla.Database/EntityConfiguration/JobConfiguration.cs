using Microsoft.EntityFrameworkCore;
using Valghalla.Application;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.EntityConfiguration
{
    internal class JobConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobEntity>(entity =>
            {
                entity.ToTable("Job");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
            });
        }
    }
}
