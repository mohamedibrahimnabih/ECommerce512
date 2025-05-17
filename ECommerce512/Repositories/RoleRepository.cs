using ECommerce512.Data;
using ECommerce512.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ECommerce512.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace ECommerce512.Repositories
{
    public class RoleRepository : Repository<IdentityRole>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        //
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
