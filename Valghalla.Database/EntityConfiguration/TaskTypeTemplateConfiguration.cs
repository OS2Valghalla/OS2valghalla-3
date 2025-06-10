using Valghalla.Database.Entities.Tables;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class TaskTypeTemplateConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskTypeTemplateEntity>(entity =>
            {
                entity.ToTable("TaskTypeTemplate");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.ShortName).HasMaxLength(50);

                entity
                     .HasMany(e => e.FileReferences)
                     .WithMany(x => x.TaskTypeTemplates)
                     .UsingEntity<TaskTypeTemplateFileEntity>(
                         l => l.HasOne(e => e.FileReference).WithMany(x => x.TaskTypeTemplateFiles).HasForeignKey(x => x.FileReferenceId),
                         r => r.HasOne(e => e.TaskTypeTemplate).WithMany(x => x.TaskTypeFileTemplates).HasForeignKey(x => x.TaskTypeTemplateId));

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<TaskTypeTemplateFileEntity>(entity =>
            {
                entity.ToTable("TaskTypeTemplateFile");

                entity.HasKey(e => new { e.TaskTypeTemplateId, e.FileReferenceId });

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
