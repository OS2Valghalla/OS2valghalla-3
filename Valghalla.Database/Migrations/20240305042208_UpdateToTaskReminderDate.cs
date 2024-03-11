using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToTaskReminderDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReminderDate",
                table: "TaskAssignment",
                newName: "TaskReminderDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskReminderDate",
                table: "TaskAssignment",
                newName: "ReminderDate");
        }
    }
}
