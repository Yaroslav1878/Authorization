using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(20230629141007)]
public class Migration011_AddScopes : Migration
{
    private readonly string _schema;

    public Migration011_AddScopes(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Insert.IntoTable("scope").InSchema(_schema)
            .Row(new { id = "manage-users" })
            .Row(new { id = "invite-users" })
            .Row(new { id = "view-user" });

        Insert.IntoTable("role_scope").InSchema(_schema)
            .Row(new { role_id = "admin", scope_id = "manage-users" })
            .Row(new { role_id = "admin", scope_id = "invite-users" })
            .Row(new { role_id = "admin", scope_id = "view-user" })
            .Row(new { role_id = "user", scope_id = "view-user" });
    }

    public override void Down()
    {
    }
}