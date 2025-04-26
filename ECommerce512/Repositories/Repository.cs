using ECommerce512.Data;
using ECommerce512.Models;
using ECommerce512.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace ECommerce512.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository()
        {
            _context = new();
            _dbSet = _context.Set<T>();
        }

        // CRUD
        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");

                return false;
            }
        }

        public bool Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");

                return false;
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");

                return false;
            }
        }

        public T? GetOne(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            return Get(expression, includes, tracked).FirstOrDefault();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> entities = _dbSet;

            // Filter
            if (expression is not null)
            {
                entities = entities.Where(expression);
            }

            if (includes is not null)
            {
                foreach (var item in includes)
                {
                    entities = entities.Include(item);
                }
            }

            if (!tracked)
            {
                entities = entities.AsNoTracking();
            }

            return entities.ToList();
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");

                return false;
            }
        }
    }
}
