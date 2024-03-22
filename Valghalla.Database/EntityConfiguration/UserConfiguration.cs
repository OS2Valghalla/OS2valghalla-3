using Microsoft.EntityFrameworkCore;
using Valghalla.Application;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.EntityConfiguration
{
    internal class UserConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("User");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
                entity.Property(e => e.Cpr).HasMaxLength(50);
                entity.Property(e => e.Cvr).HasMaxLength(50);
                entity.Property(e => e.Serial).HasMaxLength(50);
            });

            modelBuilder.Entity<UserTokenEntity>(entity =>
            {
                entity.ToTable("UserToken");

                entity.HasKey(e => e.Id);

                entity
                    .Property(e => e.Code)
                    .HasMaxLength(Constants.Validation.MaximumGeneralStringLength);
            });
        }
    }
}
