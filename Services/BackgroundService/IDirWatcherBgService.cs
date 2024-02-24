using DirWatcher.Models;

namespace DirWatcher.Services
{
    public interface IDirWatcherBgService
    {
        Task<TaskDetail> GetTaskDetailsAsync();

        Task UpdateConfig(BgConfig updatedBgConfig);
    }
}
