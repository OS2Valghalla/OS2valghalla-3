using Microsoft.EntityFrameworkCore.Migrations;
using Valghalla.Database.Schema.Data;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddReplyToMailConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ConfigurationData.InitReplyToMailConfigrationData());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
