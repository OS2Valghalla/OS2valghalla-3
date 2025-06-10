using Microsoft.EntityFrameworkCore;

using Valghalla.Application;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class TaskTypeConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskTypeEntity>(entity =>
            {
                entity.ToTable("TaskType");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.ShortName).HasMaxLength(50);

                entity
                    .HasMany(e => e.FileReferences)
                    .WithMany(x => x.TaskTypes)
                    .UsingEntity<TaskTypeFileEntity>(
                        l => l.HasOne(e => e.FileReference).WithMany(x => x.TaskTypeFiles).HasForeignKey(x => x.FileReferenceId),
                        r => r.HasOne(e => e.TaskType).WithMany(x => x.TaskTypeFiles).HasForeignKey(x => x.TaskTypeId));

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);

                entity.HasOne(e => e.TaskTypeTemplate)
                .WithMany(t => t.TaskTypes)
                .HasForeignKey(e => e.TaskTypeTemplateEntityId)
                .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TaskTypeFileEntity>(entity =>
            {
                entity.ToTable("TaskTypeFile");

                entity.HasKey(e => new { e.TaskTypeId, e.FileReferenceId });

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
