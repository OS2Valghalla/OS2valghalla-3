using Microsoft.EntityFrameworkCore;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class AuditLogConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLogEntity>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC07EF5A33E9");

                entity.ToTable("AuditLog");

                entity.HasIndex(e => new { e.Pk2value }, "IX_AuditLog_Clustered");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.EventTable).HasMaxLength(255);
                entity.Property(e => e.EventType).HasMaxLength(255);
                entity.Property(e => e.Pk2name).HasMaxLength(255);
                entity.Property(e => e.Col2name).HasMaxLength(255);
                entity.Property(e => e.Col3name).HasMaxLength(255);
            });

            modelBuilder.Entity<AuditLogEntity>()
                .HasOne(e => e.DoneByUser)
                .WithMany()
                .HasForeignKey(x => x.DoneBy)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
