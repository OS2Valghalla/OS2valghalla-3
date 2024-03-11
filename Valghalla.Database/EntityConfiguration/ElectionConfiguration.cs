using Valghalla.Database.Entities.Tables;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class ElectionConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ElectionEntity>(entity =>
            {
                entity.ToTable("Election");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);

                entity.HasOne(d => d.ElectionType).WithMany(p => p.Elections)
                   .HasForeignKey(d => new { d.ElectionTypeId });

                entity
                    .HasMany(e => e.WorkLocations)
                    .WithMany(x => x.Elections)
                    .UsingEntity<ElectionWorkLocationEntity>(
                        l => l.HasOne(e => e.WorkLocation).WithMany(x => x.ElectionWorkLocations).HasForeignKey(x => x.WorkLocationId),
                        r => r.HasOne(e => e.Election).WithMany(x => x.ElectionWorkLocations).HasForeignKey(x => x.ElectionId));

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);

                entity.HasOne(e => e.ConfirmationOfRegistrationCommunicationTemplate).WithMany().HasForeignKey(x => x.ConfirmationOfRegistrationCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.ConfirmationOfCancellationCommunicationTemplate).WithMany().HasForeignKey(x => x.ConfirmationOfCancellationCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.InvitationCommunicationTemplate).WithMany().HasForeignKey(x => x.InvitationCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.InvitationReminderCommunicationTemplate).WithMany().HasForeignKey(x => x.InvitationReminderCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.TaskReminderCommunicationTemplate).WithMany().HasForeignKey(x => x.TaskReminderCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.RetractedInvitationCommunicationTemplate).WithMany().HasForeignKey(x => x.RetractedInvitationCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.RemovedFromTaskCommunicationTemplate).WithMany().HasForeignKey(x => x.RemovedFromTaskCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.RemovedByValidationCommunicationTemplate).WithMany().HasForeignKey(x => x.RemovedByValidationCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ElectionTypeEntity>(entity =>
            {
                entity.ToTable("ElectionType");

                entity.HasKey(e => new { e.Id });
                entity.Property(e => e.Title).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);                

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<ElectionValidationRuleEntity>(entity =>
            {
                entity.ToTable("ElectionValidationRule");

                entity.HasKey(e => new { e.Id });
                entity.Property(e => e.Description).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
            });

            modelBuilder.Entity<ElectionTypeValidationRuleEntity>(entity =>
            {
                entity.ToTable("ElectionTypeValidationRule");

                entity.HasKey(e => new { e.ElectionTypeId, e.ValidationRuleId });

                entity.HasOne(d => d.ElectionType).WithMany(p => p.ValidationRules)
                 .HasForeignKey(d => new { d.ElectionTypeId })
                 .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(d => d.ValidationRule).WithMany(p => p.ValidationRules)
                .HasForeignKey(d => new { d.ValidationRuleId });
            });

            modelBuilder.Entity<ElectionWorkLocationEntity>(entity =>
            {
                entity.ToTable("ElectionWorkLocation");

                entity.HasKey(e => new { e.ElectionId, e.WorkLocationId });

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<ElectionTaskTypeCommunicationTemplateEntity>(entity =>
            {
                entity.ToTable("ElectionTaskTypeCommunicationTemplates");

                entity.HasKey(e => new { e.ElectionId, e.TaskTypeId });

                entity.HasOne(d => d.Election).WithMany(p => p.ElectionTaskTypeCommunicationTemplates)
                    .HasForeignKey(d => new { d.ElectionId })
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(d => d.TaskType).WithMany(p => p.ElectionTaskTypeCommunicationTemplates)
                    .HasForeignKey(d => new { d.TaskTypeId })
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);

                entity.HasOne(e => e.ConfirmationOfRegistrationCommunicationTemplate).WithMany().HasForeignKey(x => x.ConfirmationOfRegistrationCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.ConfirmationOfCancellationCommunicationTemplate).WithMany().HasForeignKey(x => x.ConfirmationOfCancellationCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.InvitationCommunicationTemplate).WithMany().HasForeignKey(x => x.InvitationCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.InvitationReminderCommunicationTemplate).WithMany().HasForeignKey(x => x.InvitationReminderCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.TaskReminderCommunicationTemplate).WithMany().HasForeignKey(x => x.TaskReminderCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.RetractedInvitationCommunicationTemplate).WithMany().HasForeignKey(x => x.RetractedInvitationCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.RemovedFromTaskCommunicationTemplate).WithMany().HasForeignKey(x => x.RemovedFromTaskCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.RemovedByValidationCommunicationTemplate).WithMany().HasForeignKey(x => x.RemovedByValidationCommunicationTemplateId).OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
