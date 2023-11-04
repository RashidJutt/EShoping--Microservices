using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
    {
        return await _dbcontext.Orders.Where(o => o.UserName == userName).ToListAsync();
    }
}
