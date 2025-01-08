using garage.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace garage.Areas.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Product)
                      .WithMany(p => p.Carts)
                      .HasForeignKey(e => e.ProductId);
            });

            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.Id);
            });

            builder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Cat)
                      .WithMany(c => c.Products)
                      .HasForeignKey(e => e.Catid);
            });
        }
    }
}


