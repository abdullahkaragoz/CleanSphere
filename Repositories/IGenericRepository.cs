using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetAll();
    IQueryable<T> Where(Expression<Func<T, bool>> predicate);
    ValueTask<T?> GetByIdAsync(T entity);
    ValueTask AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
