using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskTypeTemplateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaskTypeTemplateEntityId",
                table: "WorkLocationTaskTypes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TaskTypeTemplateEntityId",
                table: "TaskAssignment",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TaskTypeTemplateEntityId",
                table: "ElectionTaskTypeCommunicationTemplates",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskTypeTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    ShortName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    Description = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Payment = table.Column<int>(type: "integer", nullable: true),
                    ValidationNotRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Trusted = table.Column<bool>(type: "boolean", nullable: false),
                    SendingReminderEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypeTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTypeTemplate_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskTypeTemplate_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypeTemplateFile",
                columns: table => new
                {
                    TaskTypeTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileReferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypeTemplateFile", x => new { x.TaskTypeTemplateId, x.FileReferenceId });
                    table.ForeignKey(
                        name: "FK_TaskTypeTemplateFile_FileReference_FileReferenceId",
                        column: x => x.FileReferenceId,
                        principalTable: "FileReference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTypeTemplateFile_TaskTypeTemplate_TaskTypeTemplateId",
                        column: x => x.TaskTypeTemplateId,
                        principalTable: "TaskTypeTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTypeTemplateFile_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskTypeTemplateFile_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationTaskTypes_TaskTypeTemplateEntityId",
                table: "WorkLocationTaskTypes",
                column: "TaskTypeTemplateEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_TaskTypeTemplateEntityId",
                table: "TaskAssignment",
                column: "TaskTypeTemplateEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_TaskTypeTemplateEnti~",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "TaskTypeTemplateEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeTemplate_ChangedBy",
                table: "TaskTypeTemplate",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeTemplate_CreatedBy",
                table: "TaskTypeTemplate",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeTemplateFile_ChangedBy",
                table: "TaskTypeTemplateFile",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeTemplateFile_CreatedBy",
                table: "TaskTypeTemplateFile",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeTemplateFile_FileReferenceId",
                table: "TaskTypeTemplateFile",
                column: "FileReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_TaskTypeTemplate_Tas~",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "TaskTypeTemplateEntityId",
                principalTable: "TaskTypeTemplate",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_TaskTypeTemplate_TaskTypeTemplateEntityId",
                table: "TaskAssignment",
                column: "TaskTypeTemplateEntityId",
                principalTable: "TaskTypeTemplate",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocationTaskTypes_TaskTypeTemplate_TaskTypeTemplateEnti~",
                table: "WorkLocationTaskTypes",
                column: "TaskTypeTemplateEntityId",
                principalTable: "TaskTypeTemplate",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElectionTaskTypeCommunicationTemplates_TaskTypeTemplate_Tas~",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_TaskTypeTemplate_TaskTypeTemplateEntityId",
                table: "TaskAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocationTaskTypes_TaskTypeTemplate_TaskTypeTemplateEnti~",
                table: "WorkLocationTaskTypes");

            migrationBuilder.DropTable(
                name: "TaskTypeTemplateFile");

            migrationBuilder.DropTable(
                name: "TaskTypeTemplate");

            migrationBuilder.DropIndex(
                name: "IX_WorkLocationTaskTypes_TaskTypeTemplateEntityId",
                table: "WorkLocationTaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignment_TaskTypeTemplateEntityId",
                table: "TaskAssignment");

            migrationBuilder.DropIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_TaskTypeTemplateEnti~",
                table: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropColumn(
                name: "TaskTypeTemplateEntityId",
                table: "WorkLocationTaskTypes");

            migrationBuilder.DropColumn(
                name: "TaskTypeTemplateEntityId",
                table: "TaskAssignment");

            migrationBuilder.DropColumn(
                name: "TaskTypeTemplateEntityId",
                table: "ElectionTaskTypeCommunicationTemplates");
        }
    }
}
