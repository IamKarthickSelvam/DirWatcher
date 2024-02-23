using DirWatcher.Models;
using Microsoft.EntityFrameworkCore;

namespace DirWatcher.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BgConfig> BgConfiguration { get; set; }
    }
}
