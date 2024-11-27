using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(p=>p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        builder.Property(p => p.Stock).IsRequired();

    }
}
