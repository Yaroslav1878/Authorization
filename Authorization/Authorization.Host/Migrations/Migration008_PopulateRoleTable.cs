using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(8)]
public class Migration008_PopulateRoleTable : Migration
{
    private readonly string _schema;

    public Migration008_PopulateRoleTable(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Insert.IntoTable("role").InSchema(_schema)
            .Row(new { id = "supervisor" })
            .Row(new { id = "admin" })
            .Row(new { id = "user" })
            .Row(new { id = "unconfirmed-user" });
    }

    public override void Down()
    {
        Delete.FromTable("role").InSchema(_schema).AllRows();
    }
}