using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator.Runner.VersionTableInfo;

namespace Authorization.Host.Migrations;

[VersionTableMetaData]
public class CustomVersionTableMetadata(IPersistenceConfiguration persistenceConfiguration) : IVersionTableMetaData
{
    public string ColumnName => "Version";
    public string UniqueIndexName => "UC_Version";
    public string AppliedOnColumnName => "AppliedOn";
    public string DescriptionColumnName => "Description";
    public object ApplicationContext { get; set; } = null!;

    public string TableName => "_version_info";

    public bool OwnsSchema => true;
    public string SchemaName => persistenceConfiguration.Schema;
}