using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(2)]
public class Migration002_AddUserTable : Migration
{
    private readonly string _schema;

    public Migration002_AddUserTable(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Create.Table("user").InSchema(_schema)
            .WithColumn("id").AsInt32().Identity().PrimaryKey("user_pk")
            .WithColumn("role_name").AsString().NotNullable().ForeignKey("fk_user_role", _schema, "role", "id")
            .WithColumn("email").AsString(256).NotNullable().Unique()
            .WithColumn("first_name").AsString().NotNullable()
            .WithColumn("last_name").AsString().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("removed_at").AsDateTime().Nullable()
            .WithColumn("status").AsString().NotNullable()
            .WithColumn("avatar").AsGuid().Nullable();
    }

    public override void Down()
    {
        Delete.Table("user").InSchema(_schema);
    }
}
