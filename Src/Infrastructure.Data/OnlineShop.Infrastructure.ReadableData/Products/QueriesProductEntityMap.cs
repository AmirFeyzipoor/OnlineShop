using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Entities.Products;

namespace OnlineShop.Infrastructure.ReadableData.Products;

public class QueriesProductEntityMap : IEntityTypeConfiguration<QueriesProduct>
{
    public void Configure(EntityTypeBuilder<QueriesProduct> builder)
    {
        builder.ToTable("Products");

        builder.Property(_ => _.Id).IsRequired();
        builder.Property(_ => _.Name).IsRequired();
        builder.Property(_ => _.ManufactureEmail).IsRequired();
        builder.Property(_ => _.ProduceDate).IsRequired();
        builder.Property(_ => _.IsAvailable).IsRequired();
        builder.Property(_ => _.RegistrantId).IsRequired();
        builder.Property(_ => _.RegistrantUserName).IsRequired();
        builder.Property(_ => _.ManufacturePhone).IsRequired(false);
    }
}