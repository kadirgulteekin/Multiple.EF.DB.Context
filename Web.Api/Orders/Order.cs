namespace Web.Api.Orders;

public class Order
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public List<LineItems> LineItems { get; set; } = new();

}
