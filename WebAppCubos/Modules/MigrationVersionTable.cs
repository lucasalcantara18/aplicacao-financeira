using FluentMigrator.Runner.VersionTableInfo;

namespace WebApi.Modules.Database
{
    public class MigrationVersionTable : IVersionTableMetaData
    {
        public object ApplicationContext { get; set; }

        public bool OwnsSchema { get; }

        public string SchemaName => "";

        public string TableName => "migration_history";

        public string DescriptionColumnName => "description";

        public string UniqueIndexName => "unique_index";

        public string AppliedOnColumnName => "created_at";

        public string ColumnName => "version";

        public static IVersionTableMetaData Default => new MigrationVersionTable();
    }
}