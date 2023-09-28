using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAuditColumnsInLinkTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLink_User_ChangedBy",
                table: "TaskLink");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLink_User_CreatedBy",
                table: "TaskLink");

            migrationBuilder.DropForeignKey(
                name: "FK_TasksFilteredLink_User_ChangedBy",
                table: "TasksFilteredLink");

            migrationBuilder.DropForeignKey(
                name: "FK_TasksFilteredLink_User_CreatedBy",
                table: "TasksFilteredLink");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamLink_User_ChangedBy",
                table: "TeamLink");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamLink_User_CreatedBy",
                table: "TeamLink");

            migrationBuilder.DropIndex(
                name: "IX_TeamLink_ChangedBy",
                table: "TeamLink");

            migrationBuilder.DropIndex(
                name: "IX_TeamLink_CreatedBy",
                table: "TeamLink");

            migrationBuilder.DropIndex(
                name: "IX_TasksFilteredLink_ChangedBy",
                table: "TasksFilteredLink");

            migrationBuilder.DropIndex(
                name: "IX_TasksFilteredLink_CreatedBy",
                table: "TasksFilteredLink");

            migrationBuilder.DropIndex(
                name: "IX_TaskLink_ChangedBy",
                table: "TaskLink");

            migrationBuilder.DropIndex(
                name: "IX_TaskLink_CreatedBy",
                table: "TaskLink");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "TeamLink");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "TeamLink");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TeamLink");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TeamLink");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "TasksFilteredLink");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "TasksFilteredLink");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TasksFilteredLink");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TasksFilteredLink");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "TaskLink");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "TaskLink");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TaskLink");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TaskLink");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "TeamLink",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChangedBy",
                table: "TeamLink",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TeamLink",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "TeamLink",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "TasksFilteredLink",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChangedBy",
                table: "TasksFilteredLink",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TasksFilteredLink",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "TasksFilteredLink",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "TaskLink",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChangedBy",
                table: "TaskLink",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TaskLink",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "TaskLink",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TeamLink_ChangedBy",
                table: "TeamLink",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLink_CreatedBy",
                table: "TeamLink",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TasksFilteredLink_ChangedBy",
                table: "TasksFilteredLink",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TasksFilteredLink_CreatedBy",
                table: "TasksFilteredLink",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLink_ChangedBy",
                table: "TaskLink",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLink_CreatedBy",
                table: "TaskLink",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLink_User_ChangedBy",
                table: "TaskLink",
                column: "ChangedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLink_User_CreatedBy",
                table: "TaskLink",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TasksFilteredLink_User_ChangedBy",
                table: "TasksFilteredLink",
                column: "ChangedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TasksFilteredLink_User_CreatedBy",
                table: "TasksFilteredLink",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamLink_User_ChangedBy",
                table: "TeamLink",
                column: "ChangedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamLink_User_CreatedBy",
                table: "TeamLink",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
