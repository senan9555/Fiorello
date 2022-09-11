using Fiorello.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fiorello.DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
    }
}
