using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Valghalla.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Analyze");

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Key = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    Value = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Datatype",
                schema: "Analyze",
                columns: table => new
                {
                    DatatypeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: false, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Datatype__2B1CF62988885FAB", x => x.DatatypeId);
                });

            migrationBuilder.CreateTable(
                name: "ElectionValidationRule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionValidationRule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    JobId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListPickerType",
                schema: "Analyze",
                columns: table => new
                {
                    ListPickerTypeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ListPickerTypeId", x => x.ListPickerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ListType",
                schema: "Analyze",
                columns: table => new
                {
                    ListTypeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    View = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    Table = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ListType__5268C8CA6FE7C67D", x => x.ListTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Operator",
                schema: "Analyze",
                columns: table => new
                {
                    OperatorId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, collation: "da-DK-x-icu"),
                    Symbol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Operator__7BB12FAEDC1BDEAE", x => x.OperatorId);
                });

            migrationBuilder.CreateTable(
                name: "PrintTemplateMappings",
                columns: table => new
                {
                    Entity_EntityName = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    Entity_EntityPropertyName = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    Template_TableName = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    Template_FieldName = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintTemplateMappings", x => new { x.Entity_EntityName, x.Entity_EntityPropertyName });
                });

            migrationBuilder.CreateTable(
                name: "Query",
                schema: "Analyze",
                columns: table => new
                {
                    QueryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ListTypeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    IsTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    SystemName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    IsGlobal = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Query__5967F7DAAA5A5FF1", x => x.QueryId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    Cvr = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    Cpr = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    Serial = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Column",
                schema: "Analyze",
                columns: table => new
                {
                    ColumnId = table.Column<int>(type: "integer", nullable: false),
                    ListTypeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    ColumnName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    DatatypeId = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "((1))"),
                    IsSortable = table.Column<bool>(type: "boolean", nullable: false),
                    IsFilterable = table.Column<bool>(type: "boolean", nullable: false),
                    ListPickerTypeId = table.Column<int>(type: "integer", nullable: true, defaultValueSql: "((1))"),
                    IsViewable = table.Column<bool>(type: "boolean", nullable: false),
                    Ordinal = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "((1))"),
                    RelatedListId = table.Column<int>(type: "integer", nullable: false),
                    LookupFunction = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, collation: "da-DK-x-icu"),
                    LookupColumnId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Column__1AA1420EE98F6ED2", x => x.ColumnId);
                    table.ForeignKey(
                        name: "FK_Column_Datatype",
                        column: x => x.DatatypeId,
                        principalSchema: "Analyze",
                        principalTable: "Datatype",
                        principalColumn: "DatatypeId");
                    table.ForeignKey(
                        name: "FK_Column_ListPickerType",
                        column: x => x.ListPickerTypeId,
                        principalSchema: "Analyze",
                        principalTable: "ListPickerType",
                        principalColumn: "ListPickerTypeId");
                    table.ForeignKey(
                        name: "FK_Column_ListType",
                        column: x => x.ListTypeId,
                        principalSchema: "Analyze",
                        principalTable: "ListType",
                        principalColumn: "ListTypeId");
                });

            migrationBuilder.CreateTable(
                name: "RelatedList",
                schema: "Analyze",
                columns: table => new
                {
                    PrimaryListTypeId = table.Column<int>(type: "integer", nullable: false),
                    RelatedListTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedList", x => new { x.PrimaryListTypeId, x.RelatedListTypeId });
                    table.ForeignKey(
                        name: "FK_RelatedList_ListTypePrimary",
                        column: x => x.PrimaryListTypeId,
                        principalSchema: "Analyze",
                        principalTable: "ListType",
                        principalColumn: "ListTypeId");
                    table.ForeignKey(
                        name: "FK_RelatedList_ListTypeRelated",
                        column: x => x.RelatedListTypeId,
                        principalSchema: "Analyze",
                        principalTable: "ListType",
                        principalColumn: "ListTypeId");
                });

            migrationBuilder.CreateTable(
                name: "Filter",
                schema: "Analyze",
                columns: table => new
                {
                    FilterId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QueryId = table.Column<int>(type: "integer", nullable: false),
                    Ordinal = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Filter__3159DF6FCEECCF18", x => x.FilterId);
                    table.ForeignKey(
                        name: "FK__Filter__QueryId__395884C4",
                        column: x => x.QueryId,
                        principalSchema: "Analyze",
                        principalTable: "Query",
                        principalColumn: "QueryId");
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Area_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Area_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    Subject = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    Content = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    TemplateType = table.Column<int>(type: "integer", nullable: false),
                    IsDefaultTemplate = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationTemplate_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommunicationTemplate_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectionType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectionType_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ElectionType_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileReference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileReference_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileReference_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cpr = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    MobileNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    StreetAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    PostalCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    MunicipalityCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, collation: "da-DK-x-icu"),
                    MunicipalityCvr = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, collation: "da-DK-x-icu"),
                    MunicipalityName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    CountryCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, collation: "da-DK-x-icu"),
                    CountryName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    CoAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deceased = table.Column<bool>(type: "boolean", nullable: false),
                    Disenfranchised = table.Column<bool>(type: "boolean", nullable: false),
                    ExemptDigitalPost = table.Column<bool>(type: "boolean", nullable: false),
                    LastValidationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participant_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Participant_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participant_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialDiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialDiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialDiet_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpecialDiet_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskType",
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
                    table.PrimaryKey("PK_TaskType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskType_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskType_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, collation: "da-DK-x-icu"),
                    ShortName = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Team_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebPages",
                columns: table => new
                {
                    PageName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, collation: "da-DK-x-icu"),
                    PageInfo = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPages", x => x.PageName);
                    table.ForeignKey(
                        name: "FK_WebPages_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WebPages_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ColumnOperator",
                schema: "Analyze",
                columns: table => new
                {
                    ColumnOperatorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ColumnId = table.Column<int>(type: "integer", nullable: false),
                    OperatorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ColumnOp__FAE0D17E395F8D83", x => x.ColumnOperatorId);
                    table.ForeignKey(
                        name: "FK_ColumnOperator_Column",
                        column: x => x.ColumnId,
                        principalSchema: "Analyze",
                        principalTable: "Column",
                        principalColumn: "ColumnId");
                    table.ForeignKey(
                        name: "FK__ColumnOpe__Opera__37703C52",
                        column: x => x.OperatorId,
                        principalSchema: "Analyze",
                        principalTable: "Operator",
                        principalColumn: "OperatorId");
                });

            migrationBuilder.CreateTable(
                name: "ListTypeColumn",
                schema: "Analyze",
                columns: table => new
                {
                    ColumnId = table.Column<int>(type: "integer", nullable: false),
                    ListTypeId = table.Column<int>(type: "integer", nullable: false),
                    Ordinal = table.Column<int>(type: "integer", nullable: false),
                    RelatedListTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListTypeColumn", x => new { x.ColumnId, x.ListTypeId });
                    table.ForeignKey(
                        name: "FK_ListTypeColumn_Column",
                        column: x => x.ColumnId,
                        principalSchema: "Analyze",
                        principalTable: "Column",
                        principalColumn: "ColumnId");
                    table.ForeignKey(
                        name: "FK_ListTypeColumn_ListType",
                        column: x => x.ListTypeId,
                        principalSchema: "Analyze",
                        principalTable: "ListType",
                        principalColumn: "ListTypeId");
                });

            migrationBuilder.CreateTable(
                name: "ResultColumn",
                schema: "Analyze",
                columns: table => new
                {
                    ResultColumnId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QueryId = table.Column<int>(type: "integer", nullable: false),
                    ColumnId = table.Column<int>(type: "integer", nullable: false),
                    Ordinal = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ResultCo__AE924C90960785A6", x => x.ResultColumnId);
                    table.ForeignKey(
                        name: "FK_ResultColumn_Column",
                        column: x => x.ColumnId,
                        principalSchema: "Analyze",
                        principalTable: "Column",
                        principalColumn: "ColumnId");
                    table.ForeignKey(
                        name: "FK__ResultCol__Query__41EDCAC5",
                        column: x => x.QueryId,
                        principalSchema: "Analyze",
                        principalTable: "Query",
                        principalColumn: "QueryId");
                });

            migrationBuilder.CreateTable(
                name: "SortColumn",
                schema: "Analyze",
                columns: table => new
                {
                    SortColumnId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QueryId = table.Column<int>(type: "integer", nullable: false),
                    ColumnId = table.Column<int>(type: "integer", nullable: false),
                    Ordinal = table.Column<int>(type: "integer", nullable: false),
                    IsDecending = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SortColu__68B46505E7F3C892", x => x.SortColumnId);
                    table.ForeignKey(
                        name: "FK_SortColumn_Column",
                        column: x => x.ColumnId,
                        principalSchema: "Analyze",
                        principalTable: "Column",
                        principalColumn: "ColumnId");
                    table.ForeignKey(
                        name: "FK__SortColum__Query__43D61337",
                        column: x => x.QueryId,
                        principalSchema: "Analyze",
                        principalTable: "Query",
                        principalColumn: "QueryId");
                });

            migrationBuilder.CreateTable(
                name: "WorkLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AreaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    PostalCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    City = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkLocation_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkLocation_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkLocation_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Election",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ElectionTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    LockPeriod = table.Column<int>(type: "integer", nullable: false),
                    ElectionStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ElectionEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ElectionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    ConfirmationOfRegistrationCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConfirmationOfCancellationCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    InvitationCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    InvitationReminderCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskReminderCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    RetractedInvitationCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Election", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Election_CommunicationTemplate_ConfirmationOfCancellationCo~",
                        column: x => x.ConfirmationOfCancellationCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Election_CommunicationTemplate_ConfirmationOfRegistrationCo~",
                        column: x => x.ConfirmationOfRegistrationCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Election_CommunicationTemplate_InvitationCommunicationTempl~",
                        column: x => x.InvitationCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Election_CommunicationTemplate_InvitationReminderCommunicat~",
                        column: x => x.InvitationReminderCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Election_CommunicationTemplate_RetractedInvitationCommunica~",
                        column: x => x.RetractedInvitationCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Election_CommunicationTemplate_TaskReminderCommunicationTem~",
                        column: x => x.TaskReminderCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Election_ElectionType_ElectionTypeId",
                        column: x => x.ElectionTypeId,
                        principalTable: "ElectionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Election_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Election_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectionTypeValidationRule",
                columns: table => new
                {
                    ElectionTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValidationRuleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionTypeValidationRule", x => new { x.ElectionTypeId, x.ValidationRuleId });
                    table.ForeignKey(
                        name: "FK_ElectionTypeValidationRule_ElectionType_ElectionTypeId",
                        column: x => x.ElectionTypeId,
                        principalTable: "ElectionType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ElectionTypeValidationRule_ElectionValidationRule_Validatio~",
                        column: x => x.ValidationRuleId,
                        principalTable: "ElectionValidationRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationTemplateFile",
                columns: table => new
                {
                    CommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileReferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationTemplateFile", x => new { x.CommunicationTemplateId, x.FileReferenceId });
                    table.ForeignKey(
                        name: "FK_CommunicationTemplateFile_CommunicationTemplate_Communicati~",
                        column: x => x.CommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunicationTemplateFile_FileReference_FileReferenceId",
                        column: x => x.FileReferenceId,
                        principalTable: "FileReference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunicationTemplateFile_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommunicationTemplateFile_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectionCommitteeContactInformation",
                columns: table => new
                {
                    PageName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, collation: "da-DK-x-icu"),
                    MunicipalityName = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    ElectionResponsibleApartment = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    Address = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    City = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    TelephoneNumber = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    Email = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    DigitalPost = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    LogoFileReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionCommitteeContactInformation", x => x.PageName);
                    table.ForeignKey(
                        name: "FK_ElectionCommitteeContactInformation_FileReference_LogoFileR~",
                        column: x => x.LogoFileReferenceId,
                        principalTable: "FileReference",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ElectionCommitteeContactInformation_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ElectionCommitteeContactInformation_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageType = table.Column<int>(type: "integer", nullable: false),
                    SendType = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    ShortMessage = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationLog_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunicationLog_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommunicationLog_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantEventLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantEventLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipantEventLog_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipantEventLog_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipantEventLog_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialDietParticipant",
                columns: table => new
                {
                    SpecialDietId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialDietParticipant", x => new { x.SpecialDietId, x.ParticipantId });
                    table.ForeignKey(
                        name: "FK_SpecialDietParticipant_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialDietParticipant_SpecialDiet_SpecialDietId",
                        column: x => x.SpecialDietId,
                        principalTable: "SpecialDiet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialDietParticipant_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpecialDietParticipant_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypeFile",
                columns: table => new
                {
                    TaskTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileReferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypeFile", x => new { x.TaskTypeId, x.FileReferenceId });
                    table.ForeignKey(
                        name: "FK_TaskTypeFile_FileReference_FileReferenceId",
                        column: x => x.FileReferenceId,
                        principalTable: "FileReference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTypeFile_TaskType_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTypeFile_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskTypeFile_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMember",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMember", x => new { x.TeamId, x.ParticipantId });
                    table.ForeignKey(
                        name: "FK_TeamMember_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMember_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMember_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeamMember_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamResponsible",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamResponsible", x => new { x.TeamId, x.ParticipantId });
                    table.ForeignKey(
                        name: "FK_TeamResponsible_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamResponsible_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamResponsible_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeamResponsible_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilterColumn",
                schema: "Analyze",
                columns: table => new
                {
                    FilterColumnId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FilterId = table.Column<int>(type: "integer", nullable: false),
                    ColumnId = table.Column<int>(type: "integer", nullable: false),
                    ColumnOperatorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FilterCo__045F2EEA787BE718", x => x.FilterColumnId);
                    table.ForeignKey(
                        name: "FK_FilterColumn_Column",
                        column: x => x.ColumnId,
                        principalSchema: "Analyze",
                        principalTable: "Column",
                        principalColumn: "ColumnId");
                    table.ForeignKey(
                        name: "FK__FilterCol__Colum__3A4CA8FD",
                        column: x => x.ColumnOperatorId,
                        principalSchema: "Analyze",
                        principalTable: "ColumnOperator",
                        principalColumn: "ColumnOperatorId");
                    table.ForeignKey(
                        name: "FK__FilterCol__Filte__3B40CD36",
                        column: x => x.FilterId,
                        principalSchema: "Analyze",
                        principalTable: "Filter",
                        principalColumn: "FilterId");
                });

            migrationBuilder.CreateTable(
                name: "WorkLocationResponsible",
                columns: table => new
                {
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLocationResponsible", x => new { x.WorkLocationId, x.ParticipantId });
                    table.ForeignKey(
                        name: "FK_WorkLocationResponsible_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkLocationResponsible_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkLocationResponsible_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkLocationResponsible_WorkLocation_WorkLocationId",
                        column: x => x.WorkLocationId,
                        principalTable: "WorkLocation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkLocationTaskTypes",
                columns: table => new
                {
                    WorkLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLocationTaskTypes", x => new { x.WorkLocationId, x.TaskTypeId });
                    table.ForeignKey(
                        name: "FK_WorkLocationTaskTypes_TaskType_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkLocationTaskTypes_WorkLocation_WorkLocationId",
                        column: x => x.WorkLocationId,
                        principalTable: "WorkLocation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkLocationTeams",
                columns: table => new
                {
                    WorkLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLocationTeams", x => new { x.WorkLocationId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_WorkLocationTeams_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkLocationTeams_WorkLocation_WorkLocationId",
                        column: x => x.WorkLocationId,
                        principalTable: "WorkLocation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ElectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Pk2name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    Pk2value = table.Column<Guid>(type: "uuid", nullable: true),
                    Pk3name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    Pk3value = table.Column<Guid>(type: "uuid", nullable: true),
                    Pk4name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, collation: "da-DK-x-icu"),
                    Pk4value = table.Column<Guid>(type: "uuid", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EventTable = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    EventType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, collation: "da-DK-x-icu"),
                    EventDescription = table.Column<string>(type: "text", nullable: false, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AuditLog__3214EC07EF5A33E9", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLog_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditLog_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ElectionTaskTypeCommunicationTemplates",
                columns: table => new
                {
                    ElectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfirmationOfRegistrationCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConfirmationOfCancellationCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    InvitationCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    InvitationReminderCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskReminderCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    RetractedInvitationCommunicationTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionTaskTypeCommunicationTemplates", x => new { x.ElectionId, x.TaskTypeId });
                    table.ForeignKey(
                        name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTemplat~",
                        column: x => x.ConfirmationOfCancellationCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~1",
                        column: x => x.ConfirmationOfRegistrationCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~2",
                        column: x => x.InvitationCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~3",
                        column: x => x.InvitationReminderCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~4",
                        column: x => x.RetractedInvitationCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ElectionTaskTypeCommunicationTemplates_CommunicationTempla~5",
                        column: x => x.TaskReminderCommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ElectionTaskTypeCommunicationTemplates_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ElectionTaskTypeCommunicationTemplates_TaskType_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ElectionTaskTypeCommunicationTemplates_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ElectionTaskTypeCommunicationTemplates_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectionWorkLocation",
                columns: table => new
                {
                    ElectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionWorkLocation", x => new { x.ElectionId, x.WorkLocationId });
                    table.ForeignKey(
                        name: "FK_ElectionWorkLocation_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElectionWorkLocation_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ElectionWorkLocation_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElectionWorkLocation_WorkLocation_WorkLocationId",
                        column: x => x.WorkLocationId,
                        principalTable: "WorkLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RejectedTaskAssignment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ElectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectedTaskAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RejectedTaskAssignment_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RejectedTaskAssignment_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RejectedTaskAssignment_TaskType_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RejectedTaskAssignment_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RejectedTaskAssignment_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RejectedTaskAssignment_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RejectedTaskAssignment_WorkLocation_WorkLocationId",
                        column: x => x.WorkLocationId,
                        principalTable: "WorkLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskLink",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ElectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    HashValue = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    Value = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLink", x => new { x.Id, x.ElectionId });
                    table.ForeignKey(
                        name: "FK_TaskLink_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskLink_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskLink_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TasksFilteredLink",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ElectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    HashValue = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    Value = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksFilteredLink", x => new { x.Id, x.ElectionId });
                    table.ForeignKey(
                        name: "FK_TasksFilteredLink_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TasksFilteredLink_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TasksFilteredLink_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamLink",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ElectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    HashValue = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    Value = table.Column<string>(type: "text", nullable: true, collation: "da-DK-x-icu"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamLink", x => new { x.Id, x.ElectionId });
                    table.ForeignKey(
                        name: "FK_TeamLink_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamLink_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeamLink_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilterColumnValue",
                schema: "Analyze",
                columns: table => new
                {
                    FilterColumnValueId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FilterColumnId = table.Column<int>(type: "integer", nullable: false),
                    ValueKey = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu"),
                    Value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, collation: "da-DK-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FilterCo__E13F66683C673460", x => x.FilterColumnValueId);
                    table.ForeignKey(
                        name: "FK__FilterCol__Filte__3D2915A8",
                        column: x => x.FilterColumnId,
                        principalSchema: "Analyze",
                        principalTable: "FilterColumn",
                        principalColumn: "FilterColumnId");
                });

            migrationBuilder.CreateTable(
                name: "TaskAssignment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ElectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsed = table.Column<bool>(type: "boolean", nullable: false),
                    Accepted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAssignment", x => new { x.Id, x.ElectionId });
                    table.ForeignKey(
                        name: "FK_TaskAssignment_Election_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Election",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAssignment_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskAssignment_TaskType_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAssignment_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAssignment_User_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskAssignment_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAssignment_WorkLocationTaskTypes_WorkLocationId_TaskTyp~",
                        columns: x => new { x.WorkLocationId, x.TaskTypeId },
                        principalTable: "WorkLocationTaskTypes",
                        principalColumns: new[] { "WorkLocationId", "TaskTypeId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAssignment_WorkLocationTeams_WorkLocationId_TeamId",
                        columns: x => new { x.WorkLocationId, x.TeamId },
                        principalTable: "WorkLocationTeams",
                        principalColumns: new[] { "WorkLocationId", "TeamId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAssignment_WorkLocation_WorkLocationId",
                        column: x => x.WorkLocationId,
                        principalTable: "WorkLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Area_ChangedBy",
                table: "Area",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Area_CreatedBy",
                table: "Area",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_ChangedBy",
                table: "AuditLog",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Clustered",
                table: "AuditLog",
                columns: new[] { "ElectionId", "Pk2value", "Pk3value", "Pk4value" });

            migrationBuilder.CreateIndex(
                name: "IX_Column_DatatypeId",
                schema: "Analyze",
                table: "Column",
                column: "DatatypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Column_ListPickerTypeId",
                schema: "Analyze",
                table: "Column",
                column: "ListPickerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Column_ListTypeId",
                schema: "Analyze",
                table: "Column",
                column: "ListTypeId");

            migrationBuilder.CreateIndex(
                name: "ColumnOperator_cl",
                schema: "Analyze",
                table: "ColumnOperator",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_ColumnOperator_OperatorId",
                schema: "Analyze",
                table: "ColumnOperator",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationLog_ChangedBy",
                table: "CommunicationLog",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationLog_CreatedBy",
                table: "CommunicationLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationLog_ParticipantId",
                table: "CommunicationLog",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationTemplate_ChangedBy",
                table: "CommunicationTemplate",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationTemplate_CreatedBy",
                table: "CommunicationTemplate",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationTemplateFile_ChangedBy",
                table: "CommunicationTemplateFile",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationTemplateFile_CreatedBy",
                table: "CommunicationTemplateFile",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationTemplateFile_FileReferenceId",
                table: "CommunicationTemplateFile",
                column: "FileReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Election_ChangedBy",
                table: "Election",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Election_ConfirmationOfCancellationCommunicationTemplateId",
                table: "Election",
                column: "ConfirmationOfCancellationCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Election_ConfirmationOfRegistrationCommunicationTemplateId",
                table: "Election",
                column: "ConfirmationOfRegistrationCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Election_CreatedBy",
                table: "Election",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Election_ElectionTypeId",
                table: "Election",
                column: "ElectionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Election_InvitationCommunicationTemplateId",
                table: "Election",
                column: "InvitationCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Election_InvitationReminderCommunicationTemplateId",
                table: "Election",
                column: "InvitationReminderCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Election_RetractedInvitationCommunicationTemplateId",
                table: "Election",
                column: "RetractedInvitationCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Election_TaskReminderCommunicationTemplateId",
                table: "Election",
                column: "TaskReminderCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionCommitteeContactInformation_ChangedBy",
                table: "ElectionCommitteeContactInformation",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionCommitteeContactInformation_CreatedBy",
                table: "ElectionCommitteeContactInformation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionCommitteeContactInformation_LogoFileReferenceId",
                table: "ElectionCommitteeContactInformation",
                column: "LogoFileReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_ChangedBy",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_ConfirmationOfCancel~",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "ConfirmationOfCancellationCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_ConfirmationOfRegist~",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "ConfirmationOfRegistrationCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_CreatedBy",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_InvitationCommunicat~",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "InvitationCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_InvitationReminderCo~",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "InvitationReminderCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_RetractedInvitationC~",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "RetractedInvitationCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_TaskReminderCommunic~",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "TaskReminderCommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTaskTypeCommunicationTemplates_TaskTypeId",
                table: "ElectionTaskTypeCommunicationTemplates",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionType_ChangedBy",
                table: "ElectionType",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionType_CreatedBy",
                table: "ElectionType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionTypeValidationRule_ValidationRuleId",
                table: "ElectionTypeValidationRule",
                column: "ValidationRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionWorkLocation_ChangedBy",
                table: "ElectionWorkLocation",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionWorkLocation_CreatedBy",
                table: "ElectionWorkLocation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionWorkLocation_WorkLocationId",
                table: "ElectionWorkLocation",
                column: "WorkLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FileReference_ChangedBy",
                table: "FileReference",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FileReference_CreatedBy",
                table: "FileReference",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "Filter_cl",
                schema: "Analyze",
                table: "Filter",
                columns: new[] { "QueryId", "Ordinal" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "FilterColumn_cl",
                schema: "Analyze",
                table: "FilterColumn",
                columns: new[] { "FilterId", "ColumnId" });

            migrationBuilder.CreateIndex(
                name: "IX_FilterColumn_ColumnId",
                schema: "Analyze",
                table: "FilterColumn",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterColumn_ColumnOperatorId",
                schema: "Analyze",
                table: "FilterColumn",
                column: "ColumnOperatorId");

            migrationBuilder.CreateIndex(
                name: "FilterColumnValue_cl",
                schema: "Analyze",
                table: "FilterColumnValue",
                column: "FilterColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_ListTypeColumn_ListTypeId",
                schema: "Analyze",
                table: "ListTypeColumn",
                column: "ListTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_ChangedBy",
                table: "Participant",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_CreatedBy",
                table: "Participant",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_UserId",
                table: "Participant",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantEventLog_ChangedBy",
                table: "ParticipantEventLog",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantEventLog_CreatedBy",
                table: "ParticipantEventLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantEventLog_ParticipantId",
                table: "ParticipantEventLog",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedTaskAssignment_ChangedBy",
                table: "RejectedTaskAssignment",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedTaskAssignment_CreatedBy",
                table: "RejectedTaskAssignment",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedTaskAssignment_ElectionId",
                table: "RejectedTaskAssignment",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedTaskAssignment_ParticipantId",
                table: "RejectedTaskAssignment",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedTaskAssignment_TaskTypeId",
                table: "RejectedTaskAssignment",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedTaskAssignment_TeamId",
                table: "RejectedTaskAssignment",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedTaskAssignment_WorkLocationId",
                table: "RejectedTaskAssignment",
                column: "WorkLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FK_RelatedList_ListTypePrimary",
                schema: "Analyze",
                table: "RelatedList",
                column: "PrimaryListTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FK_RelatedList_ListTypeRelated",
                schema: "Analyze",
                table: "RelatedList",
                column: "RelatedListTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultColumn_ColumnId",
                schema: "Analyze",
                table: "ResultColumn",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultColumn_QueryId",
                schema: "Analyze",
                table: "ResultColumn",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_SortColumn_ColumnId",
                schema: "Analyze",
                table: "SortColumn",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_SortColumn_QueryId",
                schema: "Analyze",
                table: "SortColumn",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDiet_ChangedBy",
                table: "SpecialDiet",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDiet_CreatedBy",
                table: "SpecialDiet",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDietParticipant_ChangedBy",
                table: "SpecialDietParticipant",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDietParticipant_CreatedBy",
                table: "SpecialDietParticipant",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDietParticipant_ParticipantId",
                table: "SpecialDietParticipant",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_ChangedBy",
                table: "TaskAssignment",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_CreatedBy",
                table: "TaskAssignment",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_ElectionId",
                table: "TaskAssignment",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_ParticipantId",
                table: "TaskAssignment",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_TaskTypeId",
                table: "TaskAssignment",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_TeamId",
                table: "TaskAssignment",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_WorkLocationId_TaskTypeId",
                table: "TaskAssignment",
                columns: new[] { "WorkLocationId", "TaskTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignment_WorkLocationId_TeamId",
                table: "TaskAssignment",
                columns: new[] { "WorkLocationId", "TeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLink_ChangedBy",
                table: "TaskLink",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLink_CreatedBy",
                table: "TaskLink",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLink_ElectionId",
                table: "TaskLink",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLink_HashValue",
                table: "TaskLink",
                column: "HashValue");

            migrationBuilder.CreateIndex(
                name: "IX_TasksFilteredLink_ChangedBy",
                table: "TasksFilteredLink",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TasksFilteredLink_CreatedBy",
                table: "TasksFilteredLink",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TasksFilteredLink_ElectionId",
                table: "TasksFilteredLink",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksFilteredLink_HashValue",
                table: "TasksFilteredLink",
                column: "HashValue");

            migrationBuilder.CreateIndex(
                name: "IX_TaskType_ChangedBy",
                table: "TaskType",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskType_CreatedBy",
                table: "TaskType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeFile_ChangedBy",
                table: "TaskTypeFile",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeFile_CreatedBy",
                table: "TaskTypeFile",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypeFile_FileReferenceId",
                table: "TaskTypeFile",
                column: "FileReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_ChangedBy",
                table: "Team",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Team_CreatedBy",
                table: "Team",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLink_ChangedBy",
                table: "TeamLink",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLink_CreatedBy",
                table: "TeamLink",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLink_ElectionId",
                table: "TeamLink",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLink_HashValue",
                table: "TeamLink",
                column: "HashValue");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_ChangedBy",
                table: "TeamMember",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_CreatedBy",
                table: "TeamMember",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_ParticipantId",
                table: "TeamMember",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamResponsible_ChangedBy",
                table: "TeamResponsible",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TeamResponsible_CreatedBy",
                table: "TeamResponsible",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TeamResponsible_ParticipantId",
                table: "TeamResponsible",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_WebPages_ChangedBy",
                table: "WebPages",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WebPages_CreatedBy",
                table: "WebPages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocation_AreaId",
                table: "WorkLocation",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocation_ChangedBy",
                table: "WorkLocation",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocation_CreatedBy",
                table: "WorkLocation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationResponsible_ChangedBy",
                table: "WorkLocationResponsible",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationResponsible_CreatedBy",
                table: "WorkLocationResponsible",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationResponsible_ParticipantId",
                table: "WorkLocationResponsible",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationTaskTypes_TaskTypeId",
                table: "WorkLocationTaskTypes",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocationTeams_TeamId",
                table: "WorkLocationTeams",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "CommunicationLog");

            migrationBuilder.DropTable(
                name: "CommunicationTemplateFile");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "ElectionCommitteeContactInformation");

            migrationBuilder.DropTable(
                name: "ElectionTaskTypeCommunicationTemplates");

            migrationBuilder.DropTable(
                name: "ElectionTypeValidationRule");

            migrationBuilder.DropTable(
                name: "ElectionWorkLocation");

            migrationBuilder.DropTable(
                name: "FilterColumnValue",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "ListTypeColumn",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "ParticipantEventLog");

            migrationBuilder.DropTable(
                name: "PrintTemplateMappings");

            migrationBuilder.DropTable(
                name: "RejectedTaskAssignment");

            migrationBuilder.DropTable(
                name: "RelatedList",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "ResultColumn",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "SortColumn",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "SpecialDietParticipant");

            migrationBuilder.DropTable(
                name: "TaskAssignment");

            migrationBuilder.DropTable(
                name: "TaskLink");

            migrationBuilder.DropTable(
                name: "TasksFilteredLink");

            migrationBuilder.DropTable(
                name: "TaskTypeFile");

            migrationBuilder.DropTable(
                name: "TeamLink");

            migrationBuilder.DropTable(
                name: "TeamMember");

            migrationBuilder.DropTable(
                name: "TeamResponsible");

            migrationBuilder.DropTable(
                name: "WebPages");

            migrationBuilder.DropTable(
                name: "WorkLocationResponsible");

            migrationBuilder.DropTable(
                name: "ElectionValidationRule");

            migrationBuilder.DropTable(
                name: "FilterColumn",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "SpecialDiet");

            migrationBuilder.DropTable(
                name: "WorkLocationTaskTypes");

            migrationBuilder.DropTable(
                name: "WorkLocationTeams");

            migrationBuilder.DropTable(
                name: "FileReference");

            migrationBuilder.DropTable(
                name: "Election");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "ColumnOperator",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "Filter",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "TaskType");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "WorkLocation");

            migrationBuilder.DropTable(
                name: "CommunicationTemplate");

            migrationBuilder.DropTable(
                name: "ElectionType");

            migrationBuilder.DropTable(
                name: "Column",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "Operator",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "Query",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Datatype",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "ListPickerType",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "ListType",
                schema: "Analyze");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
