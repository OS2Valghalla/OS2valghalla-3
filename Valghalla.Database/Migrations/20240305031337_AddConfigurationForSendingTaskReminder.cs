﻿using Microsoft.EntityFrameworkCore.Migrations;
using Valghalla.Database.Schema.Data;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationForSendingTaskReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ConfigurationData.InitTaskReminderDayConfigrationData());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
