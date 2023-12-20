using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Entities.Identities;

namespace OnlineShop.Infrastructure.ReadableData.Identities;

public class QueriesUserEntityMap : IEntityTypeConfiguration<QueriesUser>
{
    public void Configure(EntityTypeBuilder<QueriesUser> builder)
    {
        builder.ToTable("Users");

        builder.Property(_ => _.Id).IsRequired();

        builder.Property(_ => _.UserName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(_ => _.CreationDate).IsRequired();
    }
}