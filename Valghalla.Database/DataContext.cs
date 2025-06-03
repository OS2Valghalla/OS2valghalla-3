using Valghalla.Database.Entities.Analyze;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.Entities.Views;
using Valghalla.Database.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Valghalla.Database;

public partial class DataContext : DbContext
{
    public bool IsInMemory { get; set; }
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options, bool isInMemory = false)
        : base(options)
    {
        IsInMemory = isInMemory;
    }

    #region DbSets

    #region analyze
    public virtual DbSet<ColumnEntity> Analyze_Columns { get; set; }
    public virtual DbSet<ColumnOperatorEntity> Analyze_ColumnOperators { get; set; }
    public virtual DbSet<DatatypeEntity> Analyze_Datatypes { get; set; }
    public virtual DbSet<FilterColumnValueEntity> Analyze_FilterColumnValues { get; set; }
    public virtual DbSet<FilterEntity> Analyze_Filters { get; set; }
    public virtual DbSet<FilterColumnEntity> Analyze_FilterColumns { get; set; }
    public virtual DbSet<ListPickerTypeEntity> Analyze_ListPickerTypes { get; set; }
    public virtual DbSet<ListTypeEntity> Analyze_ListTypes { get; set; }
    public virtual DbSet<ListTypeColumnEntity> Analyze_ListTypeColumns { get; set; }
    public virtual DbSet<OperatorEntity> Analyze_Operators { get; set; }
    public virtual DbSet<ResultColumnEntity> Analyze_ResultColumns { get; set; }
    public virtual DbSet<SortColumnEntity> Analyze_SortColumns { get; set; }
    public virtual DbSet<QueryEntity> Analyze_Queries { get; set; }

    #endregion

    #region views
    public virtual DbSet<ApplicationView> ApplicationView { get; set; }
    public virtual DbSet<AssignmentView> AssignmentView { get; set; }
    public virtual DbSet<BuildingView> BuildingView { get; set; }
    public virtual DbSet<CourseOccassionView> CourseOccassionView { get; set; }
    public virtual DbSet<ElectoralDistrictView> ElectoralDistrictView { get; set; }
    public virtual DbSet<ElectionView> ElectionsView { get; set; }
    public virtual DbSet<GroupView> GroupView { get; set; }
    public virtual DbSet<PersonView> PersonView { get; set; }
    public virtual DbSet<RoomView> RoomView { get; set; }
    public virtual DbSet<StaffingView> StaffingsView { get; set; }
    #endregion

    public virtual DbSet<ElectionEntity> Elections { get; set; }
    public virtual DbSet<ElectionTypeEntity> ElectionTypes { get; set; }
    public virtual DbSet<ElectionValidationRuleEntity> ElectionValidationRules { get; set; }
    public virtual DbSet<ElectionTypeValidationRuleEntity> ElectionTypeValidationRules { get; set; }
    public virtual DbSet<AreaEntity> Areas { get; set; }
    public virtual DbSet<WorkLocationEntity> WorkLocations { get; set; }

    public virtual DbSet<WorkLocationTemplateEntity> WorkLocationTemplates { get; set; }
    public virtual DbSet<WorkLocationTaskTypeEntity> WorkLocationTaskTypes { get; set; }
    public virtual DbSet<WorkLocationTeamEntity> WorkLocationTeams { get; set; }
    public virtual DbSet<WorkLocationResponsibleEntity> WorkLocationResponsibles { get; set; }
    public virtual DbSet<TeamEntity> Teams { get; set; }
    public virtual DbSet<TaskTypeEntity> TaskTypes { get; set; }
    public virtual DbSet<TaskAssignmentEntity> TaskAssignments { get; set; }
    public virtual DbSet<ParticipantEntity> Participants { get; set; }
    public virtual DbSet<ParticipantEventLogEntity> ParticipantEventLogs { get; set; }
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<AuditLogEntity> AuditLogs { get; set; }
    public virtual DbSet<ConfigurationEntity> Configurations { get; set; }
    public virtual DbSet<PrintTemplateMappingEntity> PrintTemplateMappings { get; set; }
    public virtual DbSet<FileReferenceEntity> FileReferences { get; set; }
    public virtual DbSet<FileEntity> Files { get; set; }
    public virtual DbSet<CommunicationTemplateEntity> CommunicationTemplates { get; set; }
    public virtual DbSet<CommunicationLogEntity> CommunicationLogs { get; set; }
    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
         optionsBuilder.UseNpgsql("Host=localhost;port=5432;Database=Valghalla;Username=postgres;Password=!zyxzapr3");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().UseCollation("da-DK-x-icu");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ElectionConfiguration.Configure(modelBuilder);
        AreaConfiguration.Configure(modelBuilder);
        WorkLocationConfiguration.Configure(modelBuilder);
        TeamConfiguration.Configure(modelBuilder);
        TaskTypeConfiguration.Configure(modelBuilder);
        ParticipantConfiguration.Configure(modelBuilder);
        UserConfiguration.Configure(modelBuilder);
        LinkConfiguration.Configure(modelBuilder);
        SpecialDietConfiguration.Configure(modelBuilder);

        AnalyzeConfiguration.Configure(modelBuilder);
        AuditLogConfiguration.Configure(modelBuilder);
        TaskConfiguration.Configure(modelBuilder);
        ViewsConfiguration.Configure(modelBuilder, IsInMemory);
        ConfigurationConfiguration.Configure(modelBuilder);
        PrintTemplateMappingConfiguration.Configure(modelBuilder);
        WebPageConfiguration.Configure(modelBuilder);
        FileConfiguration.Configure(modelBuilder);
        CommunicationTemplateConfiguration.Configure(modelBuilder);
        JobConfiguration.Configure(modelBuilder);
        CommunicationLogConfiguration.Configure(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
