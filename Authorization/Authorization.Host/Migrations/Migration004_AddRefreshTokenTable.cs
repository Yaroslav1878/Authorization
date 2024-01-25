using System.Data;
using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(4)]
public class Migration004_AddRefreshTokenTable : Migration
{
    private readonly string _schema;

    public Migration004_AddRefreshTokenTable(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Create.Table("refresh_token").InSchema(_schema)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey("refresh_token_pk")
            .WithColumn("client_id").AsGuid().NotNullable()
            .ForeignKey("fk_refresh_token_client", _schema, "client", "id")
            .WithColumn("user_id").AsInt32().NotNullable().ForeignKey("fk_refresh_token_user", _schema, "user", "id")
            .OnDelete(Rule.Cascade)
            .WithColumn("refresh_token").AsString().NotNullable()
            .WithColumn("revoke_reason").AsString().Nullable()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("revoked_at").AsDateTime().Nullable()
            .WithColumn("expire_at").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("refresh_token").InSchema(_schema);
    }
}