using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Entities.Identities;

namespace OnlineShop.Infrastructure.WritableData.Identities;

public class UserEntityMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(_ => _.UserName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(_ => _.CreationDate).IsRequired();

        builder.Property(_ => _.PasswordHash).IsRequired();
    }
}