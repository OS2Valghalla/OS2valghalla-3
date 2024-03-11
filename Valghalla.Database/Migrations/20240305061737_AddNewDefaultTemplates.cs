using Microsoft.EntityFrameworkCore.Migrations;
using Valghalla.Database.Schema.Data;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddNewDefaultTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DefaultCommunicationTemplatesData.InitData2());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
