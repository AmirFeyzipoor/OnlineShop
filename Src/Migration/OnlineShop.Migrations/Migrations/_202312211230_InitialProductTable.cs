using System.Data;
using FluentMigrator;

namespace OnlineShop.Migrations.Migrations;

[Migration(202312211230)]
public class _202312211230_InitialProductTable : Migration
{
    public override void Up()
    {
        Create.Table("Products")
            .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("ProduceDate").AsDateTime().NotNullable()
            .WithColumn("ManufacturePhone").AsString(13).Nullable()
            .WithColumn("ManufactureEmail").AsString().NotNullable()
            .WithColumn("IsAvailable").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("RegistrantId").AsString().NotNullable()
            .ForeignKey("FK_Products_Users",
                "Users",
                "Id").OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_Products_Users").OnTable("Products");
        Delete.Table("Products");
    }
}