namespace Valghalla.Database.Entities.Tables
{
    public partial class PrintTemplateMappingEntity
    {
        public string Entity_EntityName { get; set; } = null!;
        public string Entity_EntityPropertyName { get; set; } = null!;
        public string Template_TableName { get; set; } = null!;
        public string Template_FieldName { get; set; } = null!;
    }
}
