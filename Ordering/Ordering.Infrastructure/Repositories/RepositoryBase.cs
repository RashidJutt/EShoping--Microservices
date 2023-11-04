using Microsoft.EntityFrameworkCore;
using Ordering.Core.Common;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories;

public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
{
    protected readonly OrderDbContext _dbcontext;

    public RepositoryBase(OrderDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public async Task<T> AddAsync(T entity)
    {
        _dbcontext.Set<T>().Add(entity);
        await _dbcontext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _dbcontext.Set<T>().Remove(entity);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbcontext.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbcontext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbcontext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task UpdateAsync(T entity)
    {
        _dbcontext.Entry(entity).State = EntityState.Modified;
        await _dbcontext.SaveChangesAsync();
    }
}
