using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(9)]
public class Migration009_PopulateClientTable : Migration
{
    private readonly string _schema;

    public Migration009_PopulateClientTable(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Insert.IntoTable("client").InSchema(_schema)
            .Row(new
            {
                id = "aaaa1111-0000-0000-0000-000000000001",
                name = "WEB",
                created_at = "2023-08-25 11:05:10",
            });
    }

    public override void Down()
    {
        Delete.FromTable("client").InSchema(_schema).AllRows();
    }
}
