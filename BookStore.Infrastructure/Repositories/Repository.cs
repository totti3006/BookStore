using BookStore.Application.Interfaces.Repositories;
using BookStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            var items = _dbSet.AsNoTracking().Where(predicate);

            return await items.ToListAsync();
        }

        public async Task<T?> Get(Guid id)
        {
            var item = await _dbSet.FindAsync(id);

            return item;
        }

        public Task<int> GetCount()
        {
            return _dbSet.CountAsync();
        }

        public async Task<IEnumerable<T>> GetPaging(int pageNumber, int pageSize)
        {
            var items = _dbSet.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .AsNoTracking();

            return await items.ToListAsync();
        }

        public async Task<T?> SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            var item = await _dbSet.AsNoTracking().SingleOrDefaultAsync(predicate);

            return item;
        }

        public Task Update(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
    }
}
