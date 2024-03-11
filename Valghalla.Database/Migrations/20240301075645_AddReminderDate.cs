using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddReminderDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderDate",
                table: "TaskAssignment",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderDate",
                table: "TaskAssignment");
        }
    }
}
