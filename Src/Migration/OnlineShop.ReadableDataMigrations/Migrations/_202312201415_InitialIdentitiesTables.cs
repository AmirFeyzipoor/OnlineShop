using System.Data;
using FluentMigrator;

namespace OnlineShop.Migrations.Migrations;

[Migration(202312201545)]
public class _202312201545_InitialIdentitiesTables : Migration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsString().NotNullable().PrimaryKey()
            .WithColumn("UserName").AsString(50).NotNullable()
            .WithColumn("CreationDate").AsDateTime().NotNullable()
            .WithColumn("PasswordHash").AsString().NotNullable();

        Create.Table("UserClaims")
            .WithColumn("Id").AsInt32().Identity().NotNullable().PrimaryKey()
            .WithColumn("ClaimType").AsString().NotNullable()
            .WithColumn("ClaimValue").AsString().NotNullable()
            .WithColumn("UserId").AsString().NotNullable()
            .ForeignKey("FK_UserClaims_Users",
                "Users",
                "Id").OnDelete(Rule.Cascade);

    }

    public override void Down()
    {
        Delete.ForeignKey("FK_UserClaims_Users");
        Delete.Table("UserClaims");

        Delete.Table("Users");
    }
}