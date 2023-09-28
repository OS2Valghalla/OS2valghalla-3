using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFileStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "FileReference",
                newName: "FileName");

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "FileReference",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<byte[]>(type: "bytea", nullable: false),
                    ContentHash = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_File_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_File_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileReference_FileId",
                table: "FileReference",
                column: "FileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_ChangedBy",
                table: "File",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_File_CreatedBy",
                table: "File",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_FileReference_File_FileId",
                table: "FileReference",
                column: "FileId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileReference_File_FileId",
                table: "FileReference");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropIndex(
                name: "IX_FileReference_FileId",
                table: "FileReference");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "FileReference");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "FileReference",
                newName: "Path");
        }
    }
}
