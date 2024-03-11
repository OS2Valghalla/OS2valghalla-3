using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~4",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~5",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.AddColumn<Guid>(
                name: "RemovedByValidationCommunicationTemplateId",
                table: "ElectionTaskTypeCommunicationTemplates",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RemovedFromTaskCommunicationTemplateId",
                table: "ElectionTaskTypeCommunicationTemplates",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RemovedByValidationCommunicationTemplateId",
                table: "Election",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RemovedFromTaskCommunicationTemplateId",
                table: "Election",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_RemovedByValidationC~",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "RemovedByValidationCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_RemovedFromTaskCommu~",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "RemovedFromTaskCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Election_RemovedByValidationCommunicationTemplateId",
                table: "Election",
                column: "RemovedByValidationCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Election_RemovedFromTaskCommunicationTemplateId",
                table: "Election",
                column: "RemovedFromTaskCommunicationTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Election_CommunicationTemplate_RemovedByValidationCommunica~",
                table: "Election",
                column: "RemovedByValidationCommunicationTemplateId",
                principalTable: "CommunicationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Election_CommunicationTemplate_RemovedFromTaskCommunication~",
                table: "Election",
                column: "RemovedFromTaskCommunicationTemplateId",
                principalTable: "CommunicationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~4",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "RemovedByValidationCommunicationTemplateId",
                principalTable: "CommunicationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~5",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "RemovedFromTaskCommunicationTemplateId",
                principalTable: "CommunicationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~6",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "RetractedInvitationCommunicationTemplateId",
                principalTable: "CommunicationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~7",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "TaskReminderCommunicationTemplateId",
                principalTable: "CommunicationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Election_CommunicationTemplate_RemovedByValidationCommunica~",
                table: "Election");

            migrationBuilder.DropForeignKey(
                name: "FK_Election_CommunicationTemplate_RemovedFromTaskCommunication~",
                table: "Election");

            migrationBuilder.DropForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~4",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~5",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~6",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~7",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_RemovedByValidationC~",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_RemovedFromTaskCommu~",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Election_RemovedByValidationCommunicationTemplateId",
                table: "Election");

            migrationBuilder.DropIndex(
                name: "IX_Election_RemovedFromTaskCommunicationTemplateId",
                table: "Election");

            migrationBuilder.DropColumn(
                name: "RemovedByValidationCommunicationTemplateId",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropColumn(
                name: "RemovedFromTaskCommunicationTemplateId",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropColumn(
                name: "RemovedByValidationCommunicationTemplateId",
                table: "Election");

            migrationBuilder.DropColumn(
                name: "RemovedFromTaskCommunicationTemplateId",
                table: "Election");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~4",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "RetractedInvitationCommunicationTemplateId",
                principalTable: "CommunicationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~5",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "TaskReminderCommunicationTemplateId",
                principalTable: "CommunicationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
