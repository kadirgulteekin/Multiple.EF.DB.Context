using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Web.Api.Contracts;
using Web.Api.Orders;
using Web.Api.Products;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddDbContext<OrdersDbContext>(
    options => options.UseSqlServer(
        connectionString,
        o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName,"orders")));

builder.Services.AddDbContext<ProductsDbContext>(
    options => options.UseSqlServer(
        connectionString, o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "products")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("products", async (ProductsDbContext productsDbContext) =>
{
    return Results.Ok(await productsDbContext.Products.Select(p => p.Id).ToArrayAsync());
});

app.MapPost("orders", async (
    SubmitOrderRequest submitOrderRequest,
    ProductsDbContext productsDbContext,
    OrdersDbContext ordersDb
    ) =>
{
    var products = await productsDbContext.Products
    .Where(p => submitOrderRequest.ProductIds.Contains(p.Id))
    .AsNoTracking()
    .ToListAsync();

    if(products.Count != submitOrderRequest.ProductIds.Count)
    {
        return Results.BadRequest("Some product is missing..");
    }

    var order = new Order
    {
        Id = Guid.NewGuid(),
        TotalPrice = products.Sum(p => p.Price),
        LineItems = products.Select(p => new LineItems
        {
            Id = Guid.NewGuid(),
            ProductId = p.Id,
            Price = p.Price
        })
        .ToList()

    };
    ordersDb.Add(order);
    await ordersDb.SaveChangesAsync();
    return Results.Ok();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
