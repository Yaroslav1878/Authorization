using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(1)]
public class Migration001_AddRoleTable : Migration
{
    private readonly string _schema;

    public Migration001_AddRoleTable(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Create.Table("role").InSchema(_schema)
            .WithColumn("id").AsString().NotNullable().PrimaryKey("role_pk");
    }

    public override void Down()
    {
        Delete.Table("role").InSchema(_schema);
    }
}