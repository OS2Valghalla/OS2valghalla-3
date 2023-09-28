using Valghalla.Database.Entities.Analyze;
using Microsoft.EntityFrameworkCore;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class AnalyzeConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ColumnEntity>(entity =>
            {
                entity.HasKey(e => e.ColumnId)
                    .HasName("PK__Column__1AA1420EE98F6ED2");

                entity.ToTable("Column", "Analyze");

                entity.Property(e => e.ColumnId).ValueGeneratedNever();
                entity.Property(e => e.ColumnName).HasMaxLength(50);
                entity.Property(e => e.DatatypeId).HasDefaultValueSql("((1))");
                entity.Property(e => e.IsFilterable)
                    .IsRequired();
                entity.Property(e => e.IsSortable)
                    .IsRequired();
                entity.Property(e => e.IsViewable)
                    .IsRequired();
                entity.Property(e => e.ListPickerTypeId).HasDefaultValueSql("((1))");
                entity.Property(e => e.LookupFunction).HasMaxLength(500);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Ordinal).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Datatype).WithMany(p => p.Columns)
                    .HasForeignKey(d => d.DatatypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Column_Datatype");

                entity.HasOne(d => d.ListPickerType).WithMany(p => p.Columns)
                    .HasForeignKey(d => d.ListPickerTypeId)
                    .HasConstraintName("FK_Column_ListPickerType");

                entity.HasOne(d => d.ListType).WithMany(p => p.Columns)
                    .HasForeignKey(d => d.ListTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Column_ListType");
            });

            modelBuilder.Entity<ColumnOperatorEntity>(entity =>
            {
                entity.HasKey(e => e.ColumnOperatorId)
                    .HasName("PK__ColumnOp__FAE0D17E395F8D83");

                entity.ToTable("ColumnOperator", "Analyze");

                entity.HasIndex(e => e.ColumnId, "ColumnOperator_cl");

                entity.HasOne(d => d.Column).WithMany(p => p.ColumnOperators)
                    .HasForeignKey(d => d.ColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ColumnOperator_Column");

                entity.HasOne(d => d.Operator).WithMany(p => p.ColumnOperators)
                    .HasForeignKey(d => d.OperatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ColumnOpe__Opera__37703C52");
            });

            modelBuilder.Entity<DatatypeEntity>(entity =>
            {
                entity.HasKey(e => e.DatatypeId).HasName("PK__Datatype__2B1CF62988885FAB");

                entity.ToTable("Datatype", "Analyze");

                entity.Property(e => e.DatatypeId).ValueGeneratedNever();
                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FilterEntity>(entity =>
            {
                entity.HasKey(e => e.FilterId)
                    .HasName("PK__Filter__3159DF6FCEECCF18");

                entity.ToTable("Filter", "Analyze");

                entity.HasIndex(e => new { e.QueryId, e.Ordinal }, "Filter_cl")
                    .IsUnique();

                entity.HasOne(d => d.Query).WithMany(p => p.Filters)
                    .HasForeignKey(d => d.QueryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Filter__QueryId__395884C4");
            });

            modelBuilder.Entity<FilterColumnEntity>(entity =>
            {
                entity.HasKey(e => e.FilterColumnId)
                    .HasName("PK__FilterCo__045F2EEA787BE718");

                entity.ToTable("FilterColumn", "Analyze");

                entity.HasIndex(e => new { e.FilterId, e.ColumnId }, "FilterColumn_cl");

                entity.HasOne(d => d.Column).WithMany(p => p.FilterColumns)
                    .HasForeignKey(d => d.ColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FilterColumn_Column");

                entity.HasOne(d => d.ColumnOperator).WithMany(p => p.FilterColumns)
                    .HasForeignKey(d => d.ColumnOperatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FilterCol__Colum__3A4CA8FD");

                entity.HasOne(d => d.Filter).WithMany(p => p.FilterColumns)
                    .HasForeignKey(d => d.FilterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FilterCol__Filte__3B40CD36");
            });

            modelBuilder.Entity<FilterColumnValueEntity>(entity =>
            {
                entity.HasKey(e => e.FilterColumnValueId)
                    .HasName("PK__FilterCo__E13F66683C673460");

                entity.ToTable("FilterColumnValue", "Analyze");

                entity.HasIndex(e => e.FilterColumnId, "FilterColumnValue_cl");

                entity.Property(e => e.Value).HasMaxLength(50);
                entity.Property(e => e.ValueKey).HasMaxLength(50);

                entity.HasOne(d => d.FilterColumn).WithMany(p => p.FilterColumnValues)
                    .HasForeignKey(d => d.FilterColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FilterCol__Filte__3D2915A8");
            });

            modelBuilder.Entity<ListPickerTypeEntity>(entity =>
            {
                entity.HasKey(e => e.ListPickerTypeId).HasName("PK__ListPickerTypeId");

                entity.ToTable("ListPickerType", "Analyze");

                entity.Property(e => e.ListPickerTypeId).ValueGeneratedNever();
                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<ListTypeEntity>(entity =>
            {
                entity.HasKey(e => e.ListTypeId).HasName("PK__ListType__5268C8CA6FE7C67D");

                entity.ToTable("ListType", "Analyze");

                entity.Property(e => e.ListTypeId).ValueGeneratedNever();
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Table).HasMaxLength(50);
                entity.Property(e => e.View).HasMaxLength(50);

                entity.HasMany(d => d.PrimaryListTypes).WithMany(p => p.RelatedListTypes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RelatedList",
                        r => r.HasOne<ListTypeEntity>().WithMany()
                            .HasForeignKey("PrimaryListTypeId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_RelatedList_ListTypePrimary"),
                        l => l.HasOne<ListTypeEntity>().WithMany()
                            .HasForeignKey("RelatedListTypeId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_RelatedList_ListTypeRelated"),
                        j =>
                        {
                            j.HasKey("PrimaryListTypeId", "RelatedListTypeId");
                            j.ToTable("RelatedList", "Analyze");
                            j.HasIndex(new[] { "PrimaryListTypeId" }, "IX_FK_RelatedList_ListTypePrimary");
                            j.HasIndex(new[] { "RelatedListTypeId" }, "IX_FK_RelatedList_ListTypeRelated");
                        });

                entity.HasMany(d => d.RelatedListTypes).WithMany(p => p.PrimaryListTypes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RelatedList",
                        r => r.HasOne<ListTypeEntity>().WithMany()
                            .HasForeignKey("RelatedListTypeId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_RelatedList_ListTypeRelated"),
                        l => l.HasOne<ListTypeEntity>().WithMany()
                            .HasForeignKey("PrimaryListTypeId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_RelatedList_ListTypePrimary"),
                        j =>
                        {
                            j.HasKey("PrimaryListTypeId", "RelatedListTypeId");
                            j.ToTable("RelatedList", "Analyze");
                            j.HasIndex(new[] { "PrimaryListTypeId" }, "IX_FK_RelatedList_ListTypePrimary");
                            j.HasIndex(new[] { "RelatedListTypeId" }, "IX_FK_RelatedList_ListTypeRelated");
                        });
            });

            modelBuilder.Entity<ListTypeColumnEntity>(entity =>
            {
                entity.HasKey(e => new { e.ColumnId, e.ListTypeId });

                entity.ToTable("ListTypeColumn", "Analyze");

                entity.HasOne(d => d.Column).WithMany(p => p.ListTypeColumns)
                    .HasForeignKey(d => d.ColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListTypeColumn_Column");

                entity.HasOne(d => d.ListType).WithMany(p => p.ListTypeColumns)
                    .HasForeignKey(d => d.ListTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListTypeColumn_ListType");
            });

            modelBuilder.Entity<OperatorEntity>(entity =>
            {
                entity.HasKey(e => e.OperatorId).HasName("PK__Operator__7BB12FAEDC1BDEAE");

                entity.ToTable("Operator", "Analyze");

                entity.Property(e => e.OperatorId).ValueGeneratedNever();
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Symbol).HasMaxLength(50);
            });

            modelBuilder.Entity<QueryEntity>(entity =>
            {
                entity.HasKey(e => e.QueryId)
                    .HasName("PK__Query__5967F7DAAA5A5FF1");

                entity.ToTable("Query", "Analyze");

                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.SystemName).HasMaxLength(50);
            });

            modelBuilder.Entity<ResultColumnEntity>(entity =>
            {
                entity.HasKey(e => e.ResultColumnId)
                    .HasName("PK__ResultCo__AE924C90960785A6");

                entity.ToTable("ResultColumn", "Analyze");

                entity.HasOne(d => d.Column).WithMany(p => p.ResultColumns)
                    .HasForeignKey(d => d.ColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResultColumn_Column");

                entity.HasOne(d => d.Query).WithMany(p => p.ResultColumns)
                    .HasForeignKey(d => d.QueryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ResultCol__Query__41EDCAC5");
            });

            modelBuilder.Entity<SortColumnEntity>(entity =>
            {
                entity.HasKey(e => e.SortColumnId)
                    .HasName("PK__SortColu__68B46505E7F3C892");

                entity.ToTable("SortColumn", "Analyze");

                entity.HasOne(d => d.Column).WithMany(p => p.SortColumns)
                    .HasForeignKey(d => d.ColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SortColumn_Column");

                entity.HasOne(d => d.Query).WithMany(p => p.SortColumns)
                    .HasForeignKey(d => d.QueryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SortColum__Query__43D61337");
            });
        }
    }
}
