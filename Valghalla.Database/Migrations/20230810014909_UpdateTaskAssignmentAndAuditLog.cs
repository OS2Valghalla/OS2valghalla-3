using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaskAssignmentAndAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_User_CreatedBy",
                table: "TaskAssignment");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_User_CreatedBy",
                table: "TaskAssignment",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_User_CreatedBy",
                table: "TaskAssignment");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_User_CreatedBy",
                table: "TaskAssignment",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
