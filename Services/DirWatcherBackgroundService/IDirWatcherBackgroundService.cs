using DirWatcher.Models;

namespace DirWatcher.Services.DirWatcherBackgroundService
{
    public interface IDirWatcherBackgroundService
    {
        Task<TaskDetail> GetTaskDetailsAsync();
    }
}
