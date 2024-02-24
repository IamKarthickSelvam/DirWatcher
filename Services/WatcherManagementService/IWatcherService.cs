using DirWatcher.Models;

namespace DirWatcher.Services.WatcherManagementService
{
    public interface IWatcherService
    {
        Task<TaskDetail> GetTaskDetailsAsync();

        Task UpdateConfig(BgConfig updatedBgConfig);
    }
}
