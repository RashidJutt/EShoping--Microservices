namespace Basket.Application.Responses;

public class ShoppingCartResponse
{
    public string UserName { get; set; } = "";
    public List<ShoppingCartItemResponse> Items { get; set; }

    public ShoppingCartResponse()
    {
    }

    public ShoppingCartResponse(string userName)
    {
        UserName = userName;
    }

    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
}
