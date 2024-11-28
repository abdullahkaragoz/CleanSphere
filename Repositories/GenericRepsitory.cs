using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repositories
{
    public class GenericRepsitory<T>(AppDbContext context) : IGenericRepository<T> where T : class
    {
        protected AppDbContext Context = context;

        private readonly DbSet<T> _dbSet = context.Set<T>();

        public IQueryable<T> GetAll() => _dbSet.AsQueryable().AsNoTracking();
        
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();
        
        public async ValueTask AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async ValueTask<T?> GetByIdAsync(T entity) => await _dbSet.FindAsync(entity);
        
        public void Update(T entity) => _dbSet.Update(entity);
        
        public void Delete(T entity) => _dbSet.Remove(entity);

    }
}
