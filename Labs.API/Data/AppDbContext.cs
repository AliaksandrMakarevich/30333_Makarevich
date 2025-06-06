using Labs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Labs.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
        public DbSet<PetFood> PetFoods { get; set; }        
        public DbSet<Category> Categories { get; set; }        
    }
}
