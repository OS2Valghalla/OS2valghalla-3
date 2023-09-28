using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStringLengths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PageName",
                table: "WebPages",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Serial",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Cvr",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Cpr",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Team",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SpecialDiet",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ElectionType",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Election",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldCollation: "da-DK-x-icu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PageName",
                table: "WebPages",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Serial",
                table: "User",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Cvr",
                table: "User",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Cpr",
                table: "User",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Team",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SpecialDiet",
                type: "text",
                nullable: true,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ElectionType",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldCollation: "da-DK-x-icu");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Election",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                collation: "da-DK-x-icu",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldCollation: "da-DK-x-icu");
        }
    }
}
