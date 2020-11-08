using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models.Config
{
    public class RecommnceHobbyConfig: IEntityTypeConfiguration<RecommenceHobby>
    {
       
        public void Configure(EntityTypeBuilder<RecommenceHobby> builder)
        {
            builder.ToTable("RecommenceHobby").HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();

            builder.HasMany(x => x.ProductRecommenceHobbies)
                .WithOne(x => x.RecommenceHobby)
                .HasForeignKey(x => x.RecommenceHobbyId);
        }
    }
}
