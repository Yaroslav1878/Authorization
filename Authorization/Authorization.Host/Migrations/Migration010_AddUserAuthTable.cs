using System.Data;
using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(10)]
public class Migration010_AddUserAuthTable : Migration
{
    private readonly string _schema;

    public Migration010_AddUserAuthTable(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Create.Table("user_auth").InSchema(_schema)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey("user_auth_pk")
            .WithColumn("user_id").AsInt32().NotNullable().ForeignKey("user_auth_user_id_fk", _schema, "user", "id").OnDelete(Rule.Cascade)
            .WithColumn("auth_type").AsString().NotNullable()
            .WithColumn("subject").AsString().NotNullable()
            .WithColumn("password_hash").AsString().Nullable()
            .WithColumn("salt").AsString().Nullable()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTime().Nullable();
    }

    public override void Down()
    {
    }
}