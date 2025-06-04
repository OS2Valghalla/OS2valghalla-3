using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateIdToWorkLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Election_WorkLocationTemplates_WorkLocationTemplateEntityId",
                table: "Election");

            migrationBuilder.DropIndex(
                name: "IX_Election_WorkLocationTemplateEntityId",
                table: "Election");

            migrationBuilder.DropColumn(
                name: "WorkLocationTemplateEntityId",
                table: "Election");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkLocationTemplateId",
                table: "WorkLocation",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocation_WorkLocationTemplateId",
                table: "WorkLocation",
                column: "WorkLocationTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocation_WorkLocationTemplates_WorkLocationTemplateId",
                table: "WorkLocation",
                column: "WorkLocationTemplateId",
                principalTable: "WorkLocationTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocation_WorkLocationTemplates_WorkLocationTemplateId",
                table: "WorkLocation");

            migrationBuilder.DropIndex(
                name: "IX_WorkLocation_WorkLocationTemplateId",
                table: "WorkLocation");

            migrationBuilder.DropColumn(
                name: "WorkLocationTemplateId",
                table: "WorkLocation");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkLocationTemplateEntityId",
                table: "Election",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Election_WorkLocationTemplateEntityId",
                table: "Election",
                column: "WorkLocationTemplateEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Election_WorkLocationTemplates_WorkLocationTemplateEntityId",
                table: "Election",
                column: "WorkLocationTemplateEntityId",
                principalTable: "WorkLocationTemplates",
                principalColumn: "Id");
        }
    }
}
