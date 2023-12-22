using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Entities.Products;

namespace OnlineShop.Infrastructure.WritableData.Products;

public class ProductEntityMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.Property(_ => _.Id).IsRequired();
        builder.Property(_ => _.Name).IsRequired();
        builder.Property(_ => _.ManufactureEmail).IsRequired();
        builder.Property(_ => _.ProduceDate).IsRequired();
        builder.Property(_ => _.IsAvailable).IsRequired();
        builder.Property(_ => _.ManufacturePhone).IsRequired(false);

        builder.HasOne(_ => _.Registrant)
            .WithMany()
            .HasForeignKey(_ => _.RegistrantId)
            .IsRequired();
    }
}