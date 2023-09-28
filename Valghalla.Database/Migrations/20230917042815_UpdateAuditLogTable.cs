using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLog_Election_ElectionId",
                table: "AuditLog");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLog_User_ChangedBy",
                table: "AuditLog");

            migrationBuilder.DropIndex(
                name: "IX_AuditLog_ChangedBy",
                table: "AuditLog");

            migrationBuilder.DropIndex(
                name: "IX_AuditLog_Clustered",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "ElectionId",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "Pk3value",
                table: "AuditLog");

            migrationBuilder.RenameColumn(
                name: "Pk4value",
                table: "AuditLog",
                newName: "DoneBy");

            migrationBuilder.RenameColumn(
                name: "Pk4name",
                table: "AuditLog",
                newName: "Col3name");

            migrationBuilder.RenameColumn(
                name: "Pk3name",
                table: "AuditLog",
                newName: "Col2name");

            migrationBuilder.AddColumn<string>(
                name: "Col2value",
                table: "AuditLog",
                type: "text",
                nullable: true,
                collation: "da-DK-x-icu");

            migrationBuilder.AddColumn<string>(
                name: "Col3value",
                table: "AuditLog",
                type: "text",
                nullable: true,
                collation: "da-DK-x-icu");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Clustered",
                table: "AuditLog",
                column: "Pk2value");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_DoneBy",
                table: "AuditLog",
                column: "DoneBy");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLog_User_DoneBy",
                table: "AuditLog",
                column: "DoneBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLog_User_DoneBy",
                table: "AuditLog");

            migrationBuilder.DropIndex(
                name: "IX_AuditLog_Clustered",
                table: "AuditLog");

            migrationBuilder.DropIndex(
                name: "IX_AuditLog_DoneBy",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "Col2value",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "Col3value",
                table: "AuditLog");

            migrationBuilder.RenameColumn(
                name: "DoneBy",
                table: "AuditLog",
                newName: "Pk4value");

            migrationBuilder.RenameColumn(
                name: "Col3name",
                table: "AuditLog",
                newName: "Pk4name");

            migrationBuilder.RenameColumn(
                name: "Col2name",
                table: "AuditLog",
                newName: "Pk3name");

            migrationBuilder.AddColumn<Guid>(
                name: "ChangedBy",
                table: "AuditLog",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ElectionId",
                table: "AuditLog",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Pk3value",
                table: "AuditLog",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_ChangedBy",
                table: "AuditLog",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Clustered",
                table: "AuditLog",
                columns: new[] { "ElectionId", "Pk2value", "Pk3value", "Pk4value" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLog_Election_ElectionId",
                table: "AuditLog",
                column: "ElectionId",
                principalTable: "Election",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLog_User_ChangedBy",
                table: "AuditLog",
                column: "ChangedBy",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
