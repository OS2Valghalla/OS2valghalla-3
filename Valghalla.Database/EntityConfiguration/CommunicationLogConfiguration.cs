using Microsoft.EntityFrameworkCore;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.EntityConfiguration
{
    internal class CommunicationLogConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunicationLogEntity>(entity =>
            {
                entity.ToTable("CommunicationLog");

                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.CommunicationLogs)
                    .HasForeignKey(d => d.ParticipantId);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
