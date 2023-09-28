using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditLogTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EventDescription",
                table: "AuditLog",
                type: "text",
                nullable: true,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "da-DK-x-icu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EventDescription",
                table: "AuditLog",
                type: "text",
                nullable: false,
                defaultValue: "",
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "da-DK-x-icu");
        }
    }
}
