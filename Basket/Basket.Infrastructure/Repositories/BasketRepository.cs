using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _cache;

    public BasketRepository(IDistributedCache cache)
    {
        _cache = cache;
    }
    public async Task DeleteBasket(string userName)
    {
        await _cache.RemoveAsync(userName);
    }

    public async Task<ShoppingCart> GetBasket(string userName)
    {
        var baskit=await _cache.GetStringAsync(userName);
        if (string.IsNullOrWhiteSpace(baskit))
        {
            return null;
        }

        return JsonSerializer.Deserialize<ShoppingCart>(baskit);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
    {
        await _cache.SetStringAsync(cart.UserName,JsonSerializer.Serialize(cart));
        return await GetBasket(cart.UserName);
    }
}
