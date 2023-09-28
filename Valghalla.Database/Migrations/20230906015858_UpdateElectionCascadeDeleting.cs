using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateElectionCascadeDeleting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_Election_ElectionId",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_Election_ElectionId",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "ElectionId",
                principalTable: "Election",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
