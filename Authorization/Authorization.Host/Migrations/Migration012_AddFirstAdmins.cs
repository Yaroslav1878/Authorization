using System;
using Authorization.Domain.Configurations.Abstractions;
using FluentMigrator;

namespace Authorization.Host.Migrations;

[Migration(99)]
public class Migration012_AddFirstAdmins : Migration
{
    private readonly string _schema;

    public Migration012_AddFirstAdmins(IPersistenceConfiguration configuration)
    {
        _schema = configuration.Schema;
    }

    public override void Up()
    {
        Insert.IntoTable("user").InSchema(_schema)
            .Row(new { first_name = "Andrey", last_name = "Dusheba", email = "adusheba@axon.dev", status = "Active", role_name = "admin" })
            .Row(new { first_name = "Yaroslav", last_name = "Lysenko", email = "ylysenko@axon.dev", status = "Active", role_name = "admin" });
        

        Insert.IntoTable("user_auth").InSchema(_schema)
            .Row(new
            {
                id = Guid.NewGuid(), user_id = 1, auth_type = "Email", subject = "Password",
                password_hash = "dmU4hIvqCT2s+qNmaZZHRDpRiMPnrF2iw0OGTzmE3mo=", salt = "Jt2XBZr5a4ufQse9HapleA==",
            })
            .Row(new
            {
                id = Guid.NewGuid(), user_id = 2, auth_type = "Email", subject = "Password",
                password_hash = "IcFau3Cz3GLzmM7tgJduD8uGylqsLHi/IUweq6Tw6tY=", salt = "i/Zm/fFCnDiTS783CeE06w==",
            });
    }

    public override void Down()
    {
    }
}
