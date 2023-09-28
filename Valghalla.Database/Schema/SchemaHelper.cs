namespace Valghalla.Database.Schema
{
    internal class SchemaHelper
    {

        public static string DropDbViewTemplate(string dbView, string schema = "dbo")
        => $@"
                IF OBJECT_ID (N'{schema}.{dbView}', N'V') IS NOT NULL
                    DROP VIEW {schema}.{dbView}
                GO             
            ";
    }
}
