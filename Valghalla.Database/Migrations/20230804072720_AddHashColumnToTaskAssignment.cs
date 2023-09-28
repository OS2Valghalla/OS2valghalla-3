using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddHashColumnToTaskAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashValue",
                table: "TaskAssignment",
                type: "text",
                nullable: false,
                defaultValue: "",
                collation: "da-DK-x-icu");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_HashValue",
                table: "TaskAssignment",
                column: "HashValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskAssignment_HashValue",
                table: "TaskAssignment");

            migrationBuilder.DropColumn(
                name: "HashValue",
                table: "TaskAssignment");
        }
    }
}
