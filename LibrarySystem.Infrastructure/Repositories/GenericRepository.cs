using System.Linq.Expressions;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly LibraryDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(LibraryDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

   public async Task<IEnumerable<T>> GetManyByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges,
    params Expression<Func<T, object>>[] includes)
{
    IQueryable<T> query = _dbSet.Where(expression);

    if (!trackChanges)
    {
        query = query.AsNoTracking();
    }

    if (includes != null)
    {
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
    }

    return await query.ToListAsync();
}

    public async Task<IEnumerable<T>> GetAllAsync(bool trackChanges)
    {
        return trackChanges
        ? await _dbSet.ToListAsync()
        : await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T> GetOneByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges,params Expression<Func<T,object>>[] includes)
    {
        return trackChanges
        ? await _dbSet.SingleOrDefaultAsync(expression)
        : await _dbSet.AsNoTracking().SingleOrDefaultAsync(expression);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}