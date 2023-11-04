using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.Data;

public class OrderContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<OrderDbContext>();
        optionBuilder.UseSqlServer("Data Sourse=OrderDb");

        return new OrderDbContext(optionBuilder.Options);
    }
}
