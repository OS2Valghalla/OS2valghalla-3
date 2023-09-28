using Valghalla.Database.Entities.Tables;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class WorkLocationConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkLocationEntity>(entity =>
            {
                entity.ToTable("WorkLocation");

                entity.HasKey(e => new { e.Id });
                entity.Property(e => e.Title).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.Address).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.PostalCode).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.City).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);

                entity.HasOne(d => d.Area).WithMany(p => p.WorkLocations)
                    .HasForeignKey(d => new { d.AreaId})
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<WorkLocationResponsibleEntity>(entity =>
            {
                entity.ToTable("WorkLocationResponsible");

                entity.HasKey(e => new {  e.WorkLocationId, e.ParticipantId });

                entity.HasOne(d => d.WorkLocation).WithMany(p => p.WorkLocationResponsibles)
                    .HasForeignKey(d => new { d.WorkLocationId })
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(d => d.Participant).WithMany(p => p.WorkLocationResponsibles)
                    .HasForeignKey(d => new { d.ParticipantId })
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<WorkLocationTaskTypeEntity>(entity =>
            {
                entity.ToTable("WorkLocationTaskTypes");

                entity.HasKey(e => new { e.WorkLocationId, e.TaskTypeId });

                entity.HasOne(d => d.WorkLocation).WithMany(p => p.WorkLocationTaskTypes)
                    .HasForeignKey(d => new { d.WorkLocationId })
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(d => d.TaskType).WithMany(p => p.WorkLocationTaskTypes)
                    .HasForeignKey(d => new { d.TaskTypeId })
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<WorkLocationTeamEntity>(entity =>
            {
                entity.ToTable("WorkLocationTeams");

                entity.HasKey(e => new { e.WorkLocationId, e.TeamId });

                entity.HasOne(d => d.WorkLocation).WithMany(p => p.WorkLocationTeams)
                    .HasForeignKey(d => new { d.WorkLocationId })
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(d => d.Team).WithMany(p => p.WorkLocationTeams)
                    .HasForeignKey(d => new { d.TeamId })
                    .OnDelete(DeleteBehavior.ClientCascade);
            });
        }
    }
}
