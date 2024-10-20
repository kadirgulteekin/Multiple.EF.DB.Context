namespace Web.Api.Orders;

public class LineItems
{
    public Guid Id{ get; set; }
    public Guid ProductId { get; set; }
    public decimal Price { get; set; }
}
