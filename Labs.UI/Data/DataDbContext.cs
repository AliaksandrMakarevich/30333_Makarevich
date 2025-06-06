using Labs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Labs.UI.Data
{
    public class DataDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("");
        }

        public DbSet<PetFood> PetFoods { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
