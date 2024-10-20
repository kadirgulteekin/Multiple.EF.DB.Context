using Microsoft.EntityFrameworkCore;

namespace Web.Api.Orders;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) :
        base(options)
    { 
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<LineItems> LineItems{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders");
        modelBuilder.Entity<Order>().HasMany<LineItems>().WithOne()
            .HasForeignKey(li=>li.ProductId);
    }

}
