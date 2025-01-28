using Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories;
public interface IBaseRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IQueryable<T>>? setInclude = null,
          int pageSize = 0, int pageNumber = 0);

    long CounItems { get; }

    IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
  int skip = 0, int take = 0, bool isOrdered = false, Expression<Func<T, long>>? order = null);

    //IQueryable<T> SetSkipAndTake(IQueryable<T> entry, int skip, int take);
    Task<T> GetByAsync(Expression<Func<T, bool>> filter, bool tracked = false, Func<IQueryable<T>, IQueryable<T>>? setInclude = null);
    Task<T?> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> AttachAsync(T entity);
    Task RemoveAsync(T entity);
    Task RemoveAsync(Expression<Func<T, bool>> predicate);
    Task RemoveRange(List<T> entities);
    Task<int> SaveAsync();
    Task<bool> Exists(Expression<Func<T, bool>> filter);
    IQueryable<T> GetQuery();
}
public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly DataContext _db;
    protected DbSet<T> _dbSet;
    public long CounItems { get => _count; }
    private long _count = 0;
    public BaseRepository(DataContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }

    public IQueryable<T> GetQuery()
    {
        return _dbSet.AsQueryable();
    }
    public virtual async Task<T> GetByAsync(Expression<Func<T, bool>> filter, bool tracked = false, Func<IQueryable<T>, IQueryable<T>>? setInclude = null)
    {
        IQueryable<T> query = _dbSet;
        if (!tracked)
        {
            query = query.AsNoTracking();
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (setInclude != null)
        {
            //foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            query = setInclude(query);
            //}
        }
        return await query.FirstOrDefaultAsync();
    }
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IQueryable<T>>? setInclude = null,
        int skip = 0, int take = 0)
    {
        IQueryable<T> query = _dbSet.AsQueryable();

        query = query.AsNoTracking();
        if (filter != null)
        {
            query = _dbSet.Where(filter);
        }
        _count = await query.CountAsync();

        query = SetSkipAndTake(query, skip, take);
        if (setInclude != null)
        {
            //foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            query = setInclude(query);
            //}
        }
        return await query.ToListAsync();
    }

    public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
    int skip = 0, int take = 0, bool isOrdered = false, Expression<Func<T, long>>? order = null)
    {
        IQueryable<T> query = _dbSet.AsQueryable();

        if (filter != null)
        {
            query = _dbSet.Where(filter);
        }
        _count = query.Count();

        query = SetSkipAndTake(query, skip, take);

        if (isOrdered) { query.OrderByDescending(order); }

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        return query;
    }

    public virtual async Task<T?> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        if (await SaveAsync() > 0)
            return entity;
        return null;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _db.ChangeTracker.Clear();
        _dbSet.Update(entity);
        await SaveAsync();
        return entity;
    }


    public virtual async Task<T> AttachAsync(T entity)
    {
        //_db.ChangeTracker.Clear();
        _dbSet.Attach(entity);
        await SaveAsync();
        return entity;
    }

    public virtual async Task RemoveAsync(T entity)
    {
        if (_dbSet.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);
        _dbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task RemoveAsync(Expression<Func<T, bool>> predicate)
    {
        await _dbSet.Where(predicate).ExecuteDeleteAsync();
    }

    public async Task<int> SaveAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public async Task RemoveRange(List<T> entities)
    {
        _dbSet.RemoveRange(entities);
        await SaveAsync();
    }

    public static IQueryable<T> SetSkipAndTake(IQueryable<T> query, int skip, int take)
    {
        if (skip >= 0)
            query = query.Skip(skip);
        if (take > 0)
            query = query.Take(take);

        return query;
    }

    public async Task<bool> Exists(Expression<Func<T, bool>> filter) => await _dbSet.AnyAsync(filter);
}
