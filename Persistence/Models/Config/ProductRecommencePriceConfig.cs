using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models.Config
{
    public class ProductRecommencePriceConfig : IEntityTypeConfiguration<ProductRecommencePrice>
    {

        public void Configure(EntityTypeBuilder<ProductRecommencePrice> builder)
        {
            builder.ToTable("ProductRecommencePrice").HasKey(x => x.Id);

            builder.Property(x => x.ProductId).IsRequired();

            builder.Property(x => x.RecommencePriceId).IsRequired();
        }
    }
}
