using ECommerce512.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace ECommerce512.Repositories.IRepositories
{
    public interface IRoleRepository : IRepository<IdentityRole>
    {
    }
}
