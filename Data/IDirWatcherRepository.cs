using DirWatcher.Models;

namespace DirWatcher.Data
{
    public interface IDirWatcherRepository
    {
        Task<BgConfig> GetBgConfig();
    }
}
