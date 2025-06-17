using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocation_WorkLocationTemplates_WorkLocationTemplateId",
                table: "WorkLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocationTemplates_Area_AreaId",
                table: "WorkLocationTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocationTemplates_User_ChangedByUserId",
                table: "WorkLocationTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocationTemplates_User_CreatedByUserId",
                table: "WorkLocationTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkLocationTemplates",
                table: "WorkLocationTemplates");

            migrationBuilder.DropIndex(
                name: "IX_WorkLocationTemplates_ChangedByUserId",
                table: "WorkLocationTemplates");

            migrationBuilder.DropIndex(
                name: "IX_WorkLocationTemplates_CreatedByUserId",
                table: "WorkLocationTemplates");

            migrationBuilder.DropColumn(
                name: "ChangedByUserId",
                table: "WorkLocationTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "WorkLocationTemplates");

            migrationBuilder.RenameTable(
                name: "WorkLocationTemplates",
                newName: "WorkLocationTemplate");

            migrationBuilder.RenameIndex(
                name: "IX_WorkLocationTemplates_AreaId",
                table: "WorkLocationTemplate",
                newName: "IX_WorkLocationTemplate_AreaId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "WorkLocationTemplate",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "WorkLocationTemplate",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "WorkLocationTemplate",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "WorkLocationTemplate",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkLocationTemplate",
                table: "WorkLocationTemplate",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationTemplate_ChangedBy",
                table: "WorkLocationTemplate",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationTemplate_CreatedBy",
                table: "WorkLocationTemplate",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocation_WorkLocationTemplate_WorkLocationTemplateId",
                table: "WorkLocation",
                column: "WorkLocationTemplateId",
                principalTable: "WorkLocationTemplate",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocationTemplate_Area_AreaId",
                table: "WorkLocationTemplate",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocationTemplate_User_ChangedBy",
                table: "WorkLocationTemplate",
                column: "ChangedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocationTemplate_User_CreatedBy",
                table: "WorkLocationTemplate",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocation_WorkLocationTemplate_WorkLocationTemplateId",
                table: "WorkLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocationTemplate_Area_AreaId",
                table: "WorkLocationTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocationTemplate_User_ChangedBy",
                table: "WorkLocationTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocationTemplate_User_CreatedBy",
                table: "WorkLocationTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkLocationTemplate",
                table: "WorkLocationTemplate");

            migrationBuilder.DropIndex(
                name: "IX_WorkLocationTemplate_ChangedBy",
                table: "WorkLocationTemplate");

            migrationBuilder.DropIndex(
                name: "IX_WorkLocationTemplate_CreatedBy",
                table: "WorkLocationTemplate");

            migrationBuilder.RenameTable(
                name: "WorkLocationTemplate",
                newName: "WorkLocationTemplates");

            migrationBuilder.RenameIndex(
                name: "IX_WorkLocationTemplate_AreaId",
                table: "WorkLocationTemplates",
                newName: "IX_WorkLocationTemplates_AreaId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "WorkLocationTemplates",
                type: "text",
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "WorkLocationTemplates",
                type: "text",
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "WorkLocationTemplates",
                type: "text",
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "WorkLocationTemplates",
                type: "text",
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AddColumn<Guid>(
                name: "ChangedByUserId",
                table: "WorkLocationTemplates",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "WorkLocationTemplates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkLocationTemplates",
                table: "WorkLocationTemplates",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationTemplates_ChangedByUserId",
                table: "WorkLocationTemplates",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationTemplates_CreatedByUserId",
                table: "WorkLocationTemplates",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocation_WorkLocationTemplates_WorkLocationTemplateId",
                table: "WorkLocation",
                column: "WorkLocationTemplateId",
                principalTable: "WorkLocationTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocationTemplates_Area_AreaId",
                table: "WorkLocationTemplates",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocationTemplates_User_ChangedByUserId",
                table: "WorkLocationTemplates",
                column: "ChangedByUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocationTemplates_User_CreatedByUserId",
                table: "WorkLocationTemplates",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
