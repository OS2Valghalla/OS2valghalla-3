using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddInvitationReminderDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InvitationDate",
                table: "TaskAssignment",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InvitationReminderDate",
                table: "TaskAssignment",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationDate",
                table: "TaskAssignment");

            migrationBuilder.DropColumn(
                name: "InvitationReminderDate",
                table: "TaskAssignment");
        }
    }
}
