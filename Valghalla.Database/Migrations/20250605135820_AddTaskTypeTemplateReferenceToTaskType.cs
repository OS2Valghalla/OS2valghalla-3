using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskTypeTemplateReferenceToTaskType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaskTypeTemplateEntityId",
                table: "TaskType",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskType_TaskTypeTemplateEntityId",
                table: "TaskType",
                column: "TaskTypeTemplateEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskType_TaskTypeTemplate_TaskTypeTemplateEntityId",
                table: "TaskType",
                column: "TaskTypeTemplateEntityId",
                principalTable: "TaskTypeTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskType_TaskTypeTemplate_TaskTypeTemplateEntityId",
                table: "TaskType");

            migrationBuilder.DropIndex(
                name: "IX_TaskType_TaskTypeTemplateEntityId",
                table: "TaskType");

            migrationBuilder.DropColumn(
                name: "TaskTypeTemplateEntityId",
                table: "TaskType");
        }
    }
}
