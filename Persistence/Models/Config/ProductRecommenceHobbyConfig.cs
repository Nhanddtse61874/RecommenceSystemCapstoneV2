using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models.Config
{
    public class ProductRecommenceHobbyConfig : IEntityTypeConfiguration<ProductRecommenceHobby>
    {
        
        public void Configure(EntityTypeBuilder<ProductRecommenceHobby> builder)
        {
            builder.ToTable("ProductRecommenceHobby").HasKey(x => x.Id);

            builder.Property(x => x.RecommenceHobbyId).IsRequired();

            builder.Property(x => x.ProductId).IsRequired();

        }
    }
}
