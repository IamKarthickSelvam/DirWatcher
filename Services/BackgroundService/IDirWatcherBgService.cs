using DirWatcher.Models;

namespace DirWatcher.Services
{
    public interface IDirWatcherBgService
    {
        Task<TaskDetail> GetTaskDetailsAsync();

        void UpdateConfig(BgConfig updatedBgConfig);
    }
}
