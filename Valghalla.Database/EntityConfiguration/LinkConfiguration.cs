using Valghalla.Database.Entities.Tables;
using Microsoft.EntityFrameworkCore;

namespace Valghalla.Database.EntityConfiguration
{
    internal class LinkConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamLinkEntity>(entity =>
            {
                entity.HasKey(e => new { e.Id });

                entity.ToTable("TeamLink");
                entity.HasIndex(e => e.HashValue);
                entity.Property(e => e.Value);
            });

            modelBuilder.Entity<TaskLinkEntity>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ElectionId });

                entity.ToTable("TaskLink");
                entity.HasIndex(e => e.HashValue);
                entity.Property(e => e.Value);
            });

            modelBuilder.Entity<TasksFilteredLinkEntity>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ElectionId });

                entity.ToTable("TasksFilteredLink");
                entity.HasIndex(e => e.HashValue);
                entity.Property(e => e.Value);
            });
        }
    }
}
