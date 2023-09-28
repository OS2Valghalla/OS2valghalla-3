using Microsoft.EntityFrameworkCore;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.EntityConfiguration
{
    internal class FileConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileEntity>(entity =>
            {
                entity.ToTable("File");

                entity.HasKey(e => e.Id);

                entity
                    .HasOne(d => d.FileReference)
                    .WithOne()
                    .HasForeignKey<FileReferenceEntity>(d => d.FileId);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<FileReferenceEntity>(entity =>
            {
                entity.ToTable("FileReference");

                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
