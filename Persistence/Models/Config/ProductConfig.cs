using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Models.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product").HasKey(x => x.Id);

            builder.Property(x => x.Code).IsRequired().HasMaxLength(255);

            builder.Property(x => x.CategoryId).IsRequired();

            builder.Property(x => x.Price).IsRequired();

            builder.HasMany(x => x.ProductRecommenceHobbies)
                 .WithOne(x => x.Product)
                 .HasForeignKey(x => x.ProductId);

            builder.HasMany(x => x.ProductRecommencePrices)
                 .WithOne(x => x.Product)
                 .HasForeignKey(x => x.ProductId);

        }
    }
}
