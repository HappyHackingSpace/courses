using Microsoft.EntityFrameworkCore;

namespace HHS_ORM;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use 'Include Error Detail=true' for development 
        optionsBuilder.UseNpgsql("User ID=postgres;Password=Password123.;Host=localhost;Port=5432;Database=HHS_ORM;Include Error Detail=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define the primary key for the Category entity
        modelBuilder.Entity<Category>().HasKey(p => p.Id);

        // Define a unique index on the Name property of the Category entity
        modelBuilder.Entity<Category>().HasIndex(p => p.Name).IsUnique();

        // Configure a one-to-many relationship between Category and Product entities,
        // where each Category can have multiple Products. The foreign key is CategoryId.
        modelBuilder.Entity<Category>()
            .HasMany(p => p.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);

        // Define the primary key for the Product entity
        modelBuilder.Entity<Product>().HasKey(p => p.Id);

        // Configure the Name property in the Product entity to be required (non-nullable)
        modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired();
    }
}