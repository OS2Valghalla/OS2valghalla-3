using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkLocationTemplateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkLocationTemplateEntityId",
                table: "Election",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkLocationTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AreaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    Address = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    City = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    VoteLocation = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLocationTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkLocationTemplates_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkLocationTemplates_User_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkLocationTemplates_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Election_WorkLocationTemplateEntityId",
                table: "Election",
                column: "WorkLocationTemplateEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationTemplates_AreaId",
                table: "WorkLocationTemplates",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationTemplates_ChangedByUserId",
                table: "WorkLocationTemplates",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationTemplates_CreatedByUserId",
                table: "WorkLocationTemplates",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Election_WorkLocationTemplates_WorkLocationTemplateEntityId",
                table: "Election",
                column: "WorkLocationTemplateEntityId",
                principalTable: "WorkLocationTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Election_WorkLocationTemplates_WorkLocationTemplateEntityId",
                table: "Election");

            migrationBuilder.DropTable(
                name: "WorkLocationTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Election_WorkLocationTemplateEntityId",
                table: "Election");

            migrationBuilder.DropColumn(
                name: "WorkLocationTemplateEntityId",
                table: "Election");
        }
    }
}
