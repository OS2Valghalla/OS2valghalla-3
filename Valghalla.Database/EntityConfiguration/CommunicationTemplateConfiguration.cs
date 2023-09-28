using Valghalla.Database.Entities.Tables;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class CommunicationTemplateConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunicationTemplateEntity>(entity =>
            {
                entity.ToTable("CommunicationTemplate");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);

                entity
                    .HasMany(e => e.CommunicationTemplateFileReferences)
                    .WithMany(x => x.CommunicationTemplates)
                    .UsingEntity<CommunicationTemplateFileEntity>(
                        l => l.HasOne(e => e.FileReference).WithMany(x => x.CommunicationTemplateFiles).HasForeignKey(x => x.FileReferenceId),
                        r => r.HasOne(e => e.CommunicationTemplate).WithMany(x => x.CommunicationTemplateFiles).HasForeignKey(x => x.CommunicationTemplateId));

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<CommunicationTemplateFileEntity>(entity =>
            {
                entity.ToTable("CommunicationTemplateFile");

                entity.HasKey(e => new { e.CommunicationTemplateId, e.FileReferenceId });

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
