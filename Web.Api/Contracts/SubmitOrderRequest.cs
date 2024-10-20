using System.Security.Principal;

namespace Web.Api.Contracts;

public class SubmitOrderRequest
{
    public List<Guid> ProductIds { get; set; } = new();
}
