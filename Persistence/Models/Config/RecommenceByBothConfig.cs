using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models.Config
{
    public class RecommenceByBothConfig :IEntityTypeConfiguration<RecommenceByBoth>
    {
       

        public void Configure(EntityTypeBuilder<RecommenceByBoth> builder)
        {
            builder.ToTable("RecommenceByBoth").HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();

            builder.HasMany(x => x.ProductRecommenceByBoths)
                .WithOne(x => x.RecommenceByBoth)
                .HasForeignKey(x => x.RecommenceByBothId);
        }
    }
}
