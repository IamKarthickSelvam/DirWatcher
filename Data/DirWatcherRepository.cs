using DirWatcher.Models;
using Microsoft.EntityFrameworkCore;

namespace DirWatcher.Data
{
    public class DirWatcherRepository : IDirWatcherRepository
    {
        private readonly AppDbContext _context;

        public DirWatcherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BgConfig> GetBgConfig()
        {
            return await _context.BgConfiguration.FirstOrDefaultAsync();
        }
    }
}
