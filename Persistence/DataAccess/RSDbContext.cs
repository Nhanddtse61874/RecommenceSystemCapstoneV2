using Microsoft.EntityFrameworkCore;
using Persistence.Models;
using System.Reflection;

namespace Persistence.DataAccess
{
    public class RSDbContext : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<ProductRecommenceByBoth> ProductRecommenceByBoth { get; set; }
        public DbSet<ProductRecommenceHobby> ProductRecommenceHobby { get; set; }
        public DbSet<ProductRecommencePrice> ProductRecommencePrice { get; set; }

        public DbSet<RecommenceByBoth> RecommenceByBoth { get; set; }

        public DbSet<RecommencePrice> RecommencePrice { get; set; }

        public DbSet<RecommenceHobby> RecommenceHobby { get; set; }


        public RSDbContext(DbContextOptions options) : base(options)
        {

        }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
