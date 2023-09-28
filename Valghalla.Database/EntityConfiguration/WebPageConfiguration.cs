using Microsoft.EntityFrameworkCore;
using Valghalla.Application;
using Valghalla.Database.Entities.Tables;


namespace Valghalla.Database.EntityConfiguration
{
    internal class WebPageConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebPageEntity>(entity =>
            {
                entity.ToTable("WebPages");

                entity.HasKey(e => new { e.PageName });
                entity.Property(e => e.PageName).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);                

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<ElectionCommitteeContactInformationEntity>(entity =>
            {
                entity.ToTable("ElectionCommitteeContactInformation");

                entity.HasKey(e => new { e.PageName });
                entity.Property(e => e.PageName).HasMaxLength(100);

                entity.HasOne(d => d.LogoFileReference).WithMany(p => p.ElectionCommitteeContactInformation)
                    .HasForeignKey(d => new { d.LogoFileReferenceId })
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
