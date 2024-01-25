using System.Data;
using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(7)]
public class Migration007_AddConfirmationRequest : Migration
{
    private readonly string _schema;

    public Migration007_AddConfirmationRequest(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Create.Table("confirmation_request").InSchema(_schema)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey("confirmation_request_pk")
            .WithColumn("user_id").AsInt32().NotNullable()
            .ForeignKey("confirmation_request_user_fk", _schema, "user", "id").OnDelete(Rule.Cascade)
            .WithColumn("subject").AsString().NotNullable()
            .WithColumn("confirmation_type").AsString().NotNullable()
            .WithColumn("receiver").AsString().Nullable()
            .WithColumn("confirmed_at").AsDateTime().Nullable()
            .WithColumn("revoked_at").AsDateTime().Nullable()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("expired_at").AsDateTime().Nullable();
    }

    public override void Down()
    {
        Delete.Table("confirmation_request").InSchema(_schema);
    }
}