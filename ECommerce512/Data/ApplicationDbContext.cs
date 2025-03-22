using ECommerce512.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce512.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ECommerce512;Integrated Security=True;TrustServerCertificate=True");
        }

    }
}
