using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(5)]
public class Migration005_AddScopeTable : Migration
{
    private readonly string _schema;

    public Migration005_AddScopeTable(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Create.Table("scope").InSchema(_schema)
            .WithColumn("id").AsString().NotNullable().PrimaryKey("scope_pk");
    }

    public override void Down()
    {
        Delete.Table("scope").InSchema(_schema);
    }
}