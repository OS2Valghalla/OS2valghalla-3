using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveElectionIdInTeamLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamLink_Election_ElectionId",
                table: "TeamLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamLink",
                table: "TeamLink");

            migrationBuilder.DropIndex(
                name: "IX_TeamLink_ElectionId",
                table: "TeamLink");

            migrationBuilder.DropColumn(
                name: "ElectionId",
                table: "TeamLink");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamLink",
                table: "TeamLink",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamLink",
                table: "TeamLink");

            migrationBuilder.AddColumn<Guid>(
                name: "ElectionId",
                table: "TeamLink",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamLink",
                table: "TeamLink",
                columns: new[] { "Id", "ElectionId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamLink_ElectionId",
                table: "TeamLink",
                column: "ElectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamLink_Election_ElectionId",
                table: "TeamLink",
                column: "ElectionId",
                principalTable: "Election",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
