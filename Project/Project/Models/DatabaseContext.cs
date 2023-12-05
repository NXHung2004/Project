using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Modes
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions options):base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
