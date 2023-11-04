using Microsoft.EntityFrameworkCore;
using Ordering.Core.Common;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entity in ChangeTracker.Entries<EntityBase>())
        {
            switch (entity.State)
            {
                case EntityState.Added:
                    entity.Entity.CreatedBy = "Rashid";
                    entity.Entity.CreatedDate = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entity.Entity.LastModifiedDate = DateTime.Now;
                    entity.Entity.LastModifiedBy = "Rashid";
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
