using AllupFTB.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AllupFTB.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
    }
}
