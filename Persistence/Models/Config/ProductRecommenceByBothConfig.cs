using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Models.Config
{
    public class ProductRecommenceByBothConfig : IEntityTypeConfiguration<ProductRecommenceByBoth>
    {
       

        public void Configure(EntityTypeBuilder<ProductRecommenceByBoth> builder)
        {
            builder.ToTable("ProductRecommenceBoth").HasKey(x => x.Id);

            builder.Property(x => x.RecommenceByBothId).IsRequired();

            builder.Property(x => x.ProductId).IsRequired();
        }
    }
}
