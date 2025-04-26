using ECommerce512.Data;
using ECommerce512.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerce512.Repositories
{
    public class CategoryRepository
    {

        private readonly ApplicationDbContext _context = new();

        // CRUD
        public async Task<bool> CreateAsync(Category category)
        {
            try
            {
                await _context.AddAsync(category);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");

                return false;
            }
        }

        public bool Update(Category category)
        {
            try
            {
                _context.Update(category);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");

                return false;
            }
        }

        public bool Delete(Category category)
        {
            try
            {
                _context.Remove(category);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");

                return false;
            }
        }

        public Category? GetOne(Expression<Func<Category, bool>>? expression = null, Expression<Func<Category, object>>[]? includes = null, bool tracked = true)
        {
            return Get(expression, includes, tracked).FirstOrDefault();
        }


        public IEnumerable<Category> Get(Expression<Func<Category, bool>>? expression = null, Expression<Func<Category, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<Category> categories = _context.Categories;

            // Filter
            if(expression is not null)
            {
                categories = categories.Where(expression);
            }

            if(includes is not null)
            {
                foreach (var item in includes)
                {
                    categories = categories.Include(item);
                }
            }

            if(!tracked)
            {
                categories = categories.AsNoTracking();
            }

            return categories.ToList();
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
