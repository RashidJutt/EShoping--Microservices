namespace Basket.Core.Entities;

public class ShoppingCart
{
    public string UserName { get; set; }
    public IList<ShoppingCartItem> Items { get; set; }
    public ShoppingCart()
    {
    }
    public ShoppingCart(string userName)
    {
        UserName = userName;
    }
}
