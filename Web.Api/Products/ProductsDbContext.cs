using Microsoft.EntityFrameworkCore;

namespace Web.Api.Products;


public class ProductsDbContext : DbContext
{
	public ProductsDbContext(DbContextOptions<ProductsDbContext> options) 
		:base(options)	
	{
			
	}
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("products");

        modelBuilder.Entity<Product>()
            .HasData(SeedProducts);
    }

    private static readonly Product[] SeedProducts =
    {
        new(){ Id = Guid.NewGuid(), Name = "Product #1", Price = 100},
        new(){ Id = Guid.NewGuid(), Name = "Product #2", Price = 200},
        new(){ Id = Guid.NewGuid(), Name = "Product #3", Price = 300},
        new(){ Id = Guid.NewGuid(), Name = "Product #4", Price = 400},
        new(){ Id = Guid.NewGuid(), Name = "Product #5", Price = 500}
    };
}
