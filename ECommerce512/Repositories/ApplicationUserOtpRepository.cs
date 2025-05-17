using ECommerce512.Data;
using ECommerce512.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ECommerce512.Repositories.IRepositories;

namespace ECommerce512.Repositories
{
    public class ApplicationUserOtpRepository : Repository<ApplicationUserOTP>, IApplicationUserOtpRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserOtpRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
