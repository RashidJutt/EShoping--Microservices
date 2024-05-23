using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderDbContext context, ILogger<OrderContextSeed> logger)
    {
        if (!context.Orders.Any())
        {
            context.Orders.AddRange(GetOrders());
            await context.SaveChangesAsync();
            logger.LogInformation("Order is saved.");
        }
    }

    private static IEnumerable<Order> GetOrders()
    {
        return new List<Order>()
        {
            new Order()
            {
                UserName = "rashid",
                FirstName = "Muhammad",
                LastName = "Rashid",
                EmailAddress = "rashidjutt@gmail.com",
                TotalPrice = 1000,
                AddressLine = "Lahore",
                Country = "Pakistan",
                State = "Punjab",
                ZipCode = "23000",
                CardName = "Visa",
                CardNumber = "123412234123432",
                Expiration = "12/25",
                Cvv = "123",
                PaymentMethod = 1,
                LastModifiedBy = "rashid",
                LastModifiedDate = DateTime.Now
            }
        };
    }
}
