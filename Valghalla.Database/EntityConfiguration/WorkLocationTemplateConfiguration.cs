using Valghalla.Database.Entities.Tables;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class WorkLocationTemplateTemplateConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkLocationTemplateEntity>(entity =>
            {
                entity.ToTable("WorkLocationTemplate");

                entity.HasKey(e => new { e.Id });
                entity.Property(e => e.Title).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.Address).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.PostalCode).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.City).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

        }
    }
}
