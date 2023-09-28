using Microsoft.EntityFrameworkCore.Migrations;
using Valghalla.Database.Schema.Data;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ConfigurationData.InitConfigrationData());
            migrationBuilder.Sql(ElectionValidationRulesData.InitData());
            migrationBuilder.Sql(UserData.InitAppUserData());
            migrationBuilder.Sql(DefaultCommunicationTemplatesData.InitData());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
