using Microsoft.EntityFrameworkCore;

namespace Project.Modes
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions options):base(options) { }

        public DbSet<product> Products { get; set; }
    }
}
