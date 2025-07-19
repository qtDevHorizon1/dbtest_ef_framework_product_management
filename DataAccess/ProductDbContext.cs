using System.Data.Entity;
using EFFramework.Models;

namespace EFFramework.DataAccess
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext() : base("name=DevConnection")
        {
            // Disable proxy creation for better performance
            this.Configuration.ProxyCreationEnabled = false;
            
            // Initialize the database
            Database.SetInitializer(new ProductDbInitializer());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductHistory> ProductHistory { get; set; }
        public DbSet<ProductStats> ProductStats { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Product configuration
            modelBuilder.Entity<Product>()
                .ToTable("Products")
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductId)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal")
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Weight)
                .HasColumnType("decimal")
                .HasPrecision(10, 2);

            // Category configuration
            modelBuilder.Entity<Category>()
                .ToTable("Categories")
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Category>()
                .Property(c => c.Description)
                .HasMaxLength(200);

            // Self-referencing relationship for Categories
            modelBuilder.Entity<Category>()
                .HasOptional(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId);

            // Supplier configuration
            modelBuilder.Entity<Supplier>()
                .ToTable("Suppliers")
                .HasKey(s => s.SupplierId);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.ContactName)
                .HasMaxLength(100);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Email)
                .HasMaxLength(100);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Phone)
                .HasMaxLength(20);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Address)
                .HasMaxLength(200);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Country)
                .HasMaxLength(50);

            // ProductHistory configuration
            modelBuilder.Entity<ProductHistory>()
                .ToTable("ProductHistory")
                .HasKey(h => h.HistoryId);

            modelBuilder.Entity<ProductHistory>()
                .Property(h => h.Action)
                .IsRequired()
                .HasMaxLength(10);

            modelBuilder.Entity<ProductHistory>()
                .Property(h => h.OldPrice)
                .HasColumnType("decimal")
                .HasPrecision(18, 2);

            modelBuilder.Entity<ProductHistory>()
                .Property(h => h.NewPrice)
                .HasColumnType("decimal")
                .HasPrecision(18, 2);

            // ProductStats configuration
            modelBuilder.Entity<ProductStats>()
                .ToTable("ProductStats")
                .HasKey(s => s.StatId);

            modelBuilder.Entity<ProductStats>()
                .Property(s => s.AveragePrice)
                .HasColumnType("decimal")
                .HasPrecision(18, 2);

            modelBuilder.Entity<ProductStats>()
                .Property(s => s.TotalStockValue)
                .HasColumnType("decimal")
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class ProductDbInitializer : CreateDatabaseIfNotExists<ProductDbContext>
    {
        protected override void Seed(ProductDbContext context)
        {
            // Add some initial products
            context.Products.AddRange(new[]
            {
                new Product
                {
                    Name = "Laptop",
                    Description = "High-performance laptop with 16GB RAM",
                    Price = 999.99m,
                    CreatedDate = System.DateTime.Now
                },
                new Product
                {
                    Name = "Smartphone",
                    Description = "Latest model smartphone with 128GB storage",
                    Price = 699.99m,
                    CreatedDate = System.DateTime.Now
                },
                new Product
                {
                    Name = "Headphones",
                    Description = "Noise-cancelling wireless headphones",
                    Price = 199.99m,
                    CreatedDate = System.DateTime.Now
                }
            });

            base.Seed(context);
        }
    }
} 