using Microsoft.EntityFrameworkCore;
using Valghalla.Application;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class ParticipantConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParticipantEntity>(entity =>
            {
                entity.ToTable("Participant");

                entity.HasKey(e => new { e.Id });
                entity.Property(e => e.Cpr).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.MobileNumber).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);

                entity.Property(e => e.FirstName).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.LastName).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.StreetAddress).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.PostalCode).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.City).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.MunicipalityCode).HasMaxLength(20);
                entity.Property(e => e.MunicipalityName).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.CountryCode).HasMaxLength(20);
                entity.Property(e => e.CountryName).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.CoAddress).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Participant)
                    .HasForeignKey<ParticipantEntity>(d => d.UserId);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });

            modelBuilder.Entity<ParticipantEventLogEntity>(entity =>
            {
                entity.ToTable("ParticipantEventLog");

                entity.HasKey(e => new { e.Id });

                entity.HasOne(d => d.Participant).WithMany(p => p.ParticipantEventLogs)
                    .HasForeignKey(d => new { d.ParticipantId })
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
                entity.HasOne(e => e.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedBy);
            });
        }
    }
}
