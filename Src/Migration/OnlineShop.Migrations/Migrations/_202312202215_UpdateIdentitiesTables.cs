using FluentMigrator;

namespace OnlineShop.Migrations.Migrations;

[Migration(202312202215)]
public class _202312202215_UpdateIdentitiesTables : Migration
{
    public override void Up()
    {
        Alter.Table("Users")
            .AddColumn("NormalizedUserName").AsString(50).Nullable()
            .AddColumn("Email").AsString().Nullable()
            .AddColumn("NormalizedEmail").AsString().Nullable()
            .AddColumn("EmailConfirmed").AsBoolean().NotNullable().WithDefaultValue(false)
            .AddColumn("SecurityStamp").AsString().Nullable()
            .AddColumn("ConcurrencyStamp").AsString().Nullable()
            .AddColumn("PhoneNumber").AsString().Nullable()
            .AddColumn("PhoneNumberConfirmed").AsBoolean().NotNullable().WithDefaultValue(false)
            .AddColumn("TwoFactorEnabled").AsBoolean().NotNullable().WithDefaultValue(false)
            .AddColumn("LockoutEnabled").AsBoolean().NotNullable().WithDefaultValue(false)
            .AddColumn("LockoutEnd").AsDateTimeOffset().Nullable()
            .AddColumn("AccessFailedCount").AsInt32().Nullable();
    }

    public override void Down()
    {
        Delete.Column("NormalizedUserName").FromTable("Users");
        Delete.Column("Email").FromTable("Users");
        Delete.Column("NormalizedEmail").FromTable("Users");
        Delete.Column("EmailConfirmed").FromTable("Users");
        Delete.Column("SecurityStamp").FromTable("Users");
        Delete.Column("ConcurrencyStamp").FromTable("Users");
        Delete.Column("PhoneNumber").FromTable("Users");
        Delete.Column("PhoneNumberConfirmed").FromTable("Users");
        Delete.Column("TwoFactorEnabled").FromTable("Users");
        Delete.Column("LockoutEnabled").FromTable("Users");
        Delete.Column("LockoutEnd").FromTable("Users");
        Delete.Column("AccessFailedCount").FromTable("Users");
    }
}