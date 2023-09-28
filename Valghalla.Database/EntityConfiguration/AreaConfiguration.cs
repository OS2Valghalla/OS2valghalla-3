using Microsoft.EntityFrameworkCore;
using Valghalla.Application;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class AreaConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AreaEntity>(entity =>
            {
                entity.ToTable("Area");

                entity.HasKey(e => new { e.Id });

                entity.Property(e => e.Name).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
