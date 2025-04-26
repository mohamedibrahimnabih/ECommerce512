using ECommerce512.Models;
using System.Linq.Expressions;

namespace ECommerce512.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<bool> CreateAsync(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        T? GetOne(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        IEnumerable<T> Get(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        Task<bool> CommitAsync();
    }
}
