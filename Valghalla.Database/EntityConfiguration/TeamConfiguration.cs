using Microsoft.EntityFrameworkCore;
using Valghalla.Application;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.EntityConfiguration
{
    internal class TeamConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamEntity>(entity =>
            {
                entity.ToTable("Team");

                entity.HasKey(e => new { e.Id });
                entity.Property(e => e.Name).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);

                entity
                    .HasMany(e => e.MemberParticipants)
                    .WithMany(x => x.TeamForMembers)
                    .UsingEntity<TeamMemberEntity>(
                        l => l.HasOne(e => e.Participant).WithMany(x => x.TeamMembers).HasForeignKey(x => x.ParticipantId),
                        r => r.HasOne(e => e.Team).WithMany(x => x.TeamMembers).HasForeignKey(x => x.TeamId));

                entity
                    .HasMany(e => e.ResponsibleParticipants)
                    .WithMany(x => x.TeamForResponsibles)
                    .UsingEntity<TeamResponsibleEntity>(
                        l => l.HasOne(e => e.Participant).WithMany(x => x.TeamResponsibles).HasForeignKey(x => x.ParticipantId),
                        r => r.HasOne(e => e.Team).WithMany(x => x.TeamResponsibles).HasForeignKey(x => x.TeamId));

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<TeamResponsibleEntity>(entity =>
            {
                entity.ToTable("TeamResponsible");

                entity.HasKey(e => new { e.TeamId, e.ParticipantId });             

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<TeamMemberEntity>(entity =>
            {
                entity.ToTable("TeamMember");

                entity.HasKey(e => new { e.TeamId, e.ParticipantId });

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
