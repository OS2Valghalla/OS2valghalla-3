using Valghalla.Database.Entities.Tables;
using Microsoft.EntityFrameworkCore;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class TaskConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskAssignmentEntity>(entity =>
            {
                entity.ToTable("TaskAssignment");

                entity.HasKey(e => new { e.Id, e.ElectionId });
                entity.HasIndex(e => e.HashValue);

                entity.HasOne(d => d.Participant).WithMany(p => p.TaskAssignments)
                    .HasForeignKey(d => new { d.ParticipantId });

                entity.HasOne(d => d.WorkLocationTeam).WithMany(p => p.TaskAssignments)
                    .HasForeignKey(d => new { d.WorkLocationId, d.TeamId });

                entity.HasOne(d => d.WorkLocationTaskType).WithMany(p => p.TaskAssignments)
                    .HasForeignKey(d => new { d.WorkLocationId, d.TaskTypeId });

                entity.HasOne(d => d.TaskType).WithMany(p => p.TaskAssignments)
                    .HasForeignKey(d => new { d.TaskTypeId });

                entity.HasOne(e => e.WorkLocation).WithMany().HasForeignKey(e => e.WorkLocationId);
                entity.HasOne(e => e.Team).WithMany().HasForeignKey(e => e.TeamId);
                entity.HasOne(e => e.Election).WithMany().HasForeignKey(e => e.ElectionId);

                entity
                    .HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity
                    .HasOne(e => e.ChangedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.ChangedBy)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<RejectedTaskAssignmentEntity>(entity =>
            {
                entity.ToTable("RejectedTaskAssignment");

                entity.HasKey(e => new { e.Id });

                entity.HasOne(e => e.Participant).WithMany().HasForeignKey(e => e.ParticipantId);
                entity.HasOne(e => e.WorkLocation).WithMany().HasForeignKey(e => e.WorkLocationId);
                entity.HasOne(e => e.TaskType).WithMany().HasForeignKey(e => e.TaskTypeId);
                entity.HasOne(e => e.Team).WithMany().HasForeignKey(e => e.TeamId);
                entity.HasOne(e => e.Election).WithMany().HasForeignKey(e => e.ElectionId);
                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
