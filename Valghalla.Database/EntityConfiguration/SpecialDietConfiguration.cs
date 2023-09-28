using Microsoft.EntityFrameworkCore;
using Valghalla.Application;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.EntityConfiguration
{
    internal class SpecialDietConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpecialDietEntity>(entity =>
            {
                entity.ToTable("SpecialDiet");
                entity.HasKey(e => new { e.Id });

                entity.Property(e => e.Title).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);

                entity
                    .HasMany(e => e.Participants)
                    .WithMany(x => x.SpecialDiets)
                    .UsingEntity<SpecialDietParticipantEntity>(
                        l => l.HasOne(e => e.Participant).WithMany(x => x.SpecialDietParticipants).HasForeignKey(x => x.ParticipantId),
                        r => r.HasOne(e => e.SpecialDiet).WithMany(x => x.SpecialDietParticipants).HasForeignKey(x => x.SpecialDietId));

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<SpecialDietParticipantEntity>(entity =>
            {
                entity.ToTable("SpecialDietParticipant");

                entity.HasKey(e => new { e.SpecialDietId, e.ParticipantId });

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
