using FluentMigrator;

namespace OnlineShop.Migrations.Migrations;

[Migration(202312202300)]
public class _202312202300_UpdateIdentitiesTables : Migration
{
    public override void Up()
    {
        Delete.Column("PasswordHash").FromTable("Users");
    }

    public override void Down()
    {
        Alter.Table("Users").AddColumn("PasswordHash").AsString().NotNullable();
    }
}