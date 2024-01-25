using System.Data;
using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(6)]
public class Migration006_AddRoleScopeTable : Migration
{
    private readonly string _schema;

    public Migration006_AddRoleScopeTable(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Create.Table("role_scope").InSchema(_schema)
            .WithColumn("role_id").AsString().NotNullable().ForeignKey("fk_role_scope_role", _schema, "role", "id")
            .WithColumn("scope_id").AsString().NotNullable().ForeignKey("fk_role_scope_scope", _schema, "scope", "id")
            .OnUpdate(Rule.Cascade);

        Create.PrimaryKey("role_scope_pk")
            .OnTable("role_scope").WithSchema(_schema)
            .Columns("role_id", "scope_id");
    }

    public override void Down()
    {
        Delete.Table("role_scope").InSchema(_schema);
    }
}