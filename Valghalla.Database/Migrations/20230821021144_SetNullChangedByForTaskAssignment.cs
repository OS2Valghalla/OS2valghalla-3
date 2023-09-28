using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class SetNullChangedByForTaskAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_User_ChangedBy",
                table: "TaskAssignment");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_User_ChangedBy",
                table: "TaskAssignment",
                column: "ChangedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_User_ChangedBy",
                table: "TaskAssignment");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_User_ChangedBy",
                table: "TaskAssignment",
                column: "ChangedBy",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
