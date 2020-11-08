using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models.Config
{
    public class RecommencePriceConfig : IEntityTypeConfiguration<RecommencePrice>
    {
       

        public void Configure(EntityTypeBuilder<RecommencePrice> builder)
        {
            builder.ToTable("RecommencePrice").HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();

            builder.HasMany(x => x.ProductRecommencePrices)
                .WithOne(x => x.RecommencePrice)
                .HasForeignKey(x => x.RecommencePriceId);
        }
    }
}
