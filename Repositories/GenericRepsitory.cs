using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repositories
{
    public class GenericRepsitory<T, TId>(AppDbContext context)
        : IGenericRepository<T,TId> where T
        : BaseEntity<TId> where TId : struct
    {
        protected AppDbContext Context = context;

        private readonly DbSet<T> _dbSet = context.Set<T>();

        public Task<bool> AnyAsync(TId id) => _dbSet.AnyAsync(e => e.Id.Equals(id));

        public IQueryable<T> GetAll() => _dbSet.AsQueryable().AsNoTracking();

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();

        public async ValueTask AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async ValueTask<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

    }
}
